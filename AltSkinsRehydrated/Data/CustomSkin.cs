using Newtonsoft.Json;
using Nick;
using SMU.Utilities;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AltSkinsRehydrated.Data
{
    class CustomSkin
    {
        public string filePath;

        public string name;
        public string skinId;
        public string characterId;

        public string smallIconPath;
        public string medIconPath;
        public string largeIconPath;

        public string smallIconId;
        public string medIconId;
        public string largeIconId;

        public CustomSkinMetadata metadata;
        public string AssetBundlePath;
        public string ScenePath;
        public AssetBundle bundle;
        public Scene scene;
        public bool loading = false;

        public CustomSkin(string path)
        {
            filePath = path;

            using (var archive = ZipFile.OpenRead(path))
            {
                var jsonEntry = archive.Entries.First(x => x.Name == "package.json");
                if (jsonEntry != null)
                {
                    var stream = new StreamReader(jsonEntry.Open(), Encoding.Default);
                    string jsonString = stream.ReadToEnd();
                    metadata = JsonConvert.DeserializeObject<CustomSkinMetadata>(jsonString);

                    characterId = metadata.characterID;
                    name = metadata.skinName;
                    skinId = metadata.skinID;

                    metadata.portraits.TryGetValue("portrait_small", out smallIconPath);
                    metadata.portraits.TryGetValue("portrait_medium", out medIconPath);
                    metadata.portraits.TryGetValue("portrait", out largeIconPath);

                    smallIconId = $"CUSTOM_{characterId}/{skinId}/{smallIconPath}";
                    medIconId = $"CUSTOM_{characterId}/{skinId}/{medIconPath}";
                    largeIconId = $"CUSTOM_{characterId}/{skinId}/{largeIconPath}";

                    AssetBundlePath = metadata.assetBundlePath;
                    ScenePath = metadata.scenePath;
                }
            }
        }

        public void LoadAssetBundle(AgentLoading.LoadState loadstate)
        {
            if (bundle == null && !loading)
                SharedCoroutineStarter.StartCoroutine(LoadAssetBundleCoroutine(loadstate));
        }

        public void UnloadAssetBundle()
        {
            if (bundle == null || scene == null || loading) return;

            SceneManager.UnloadSceneAsync(scene);
            bundle.Unload(true);
        }

        IEnumerator LoadAssetBundleCoroutine(AgentLoading.LoadState loadstate)
        {
            AltSkinsPlugin.LogInfo($"Loading Assetbundle for {name}...");
            loading = true;
            using (var zip = ZipFile.OpenRead(filePath))
            {
                var stream = new MemoryStream();
                var entry = zip.GetEntry(AssetBundlePath);
                if (entry == null)
                {
                    AltSkinsPlugin.LogError("Failed to load AssetBundle. Aborting!");
                    yield break;
                }
                entry.Open().CopyTo(stream);
                var request = AssetBundle.LoadFromStreamAsync(stream);
                loadstate.op = request;

                while (!request.isDone) yield return null;
                if (!request.assetBundle)
                {
                    AltSkinsPlugin.LogError("Failed to load AssetBundle. Aborting!");
                    yield break;
                }
                loadstate.phase = AgentLoading.LoadPhase.Loading;
                bundle = request.assetBundle;
                var sceneLoad = SceneManager.LoadSceneAsync(ScenePath, LoadSceneMode.Additive);
                loadstate.op = sceneLoad;

                while (!sceneLoad.isDone) yield return null;
                scene = SceneManager.GetSceneByPath(ScenePath);
            }
            AltSkinsPlugin.LogInfo($"{name} loaded!");
            loading = false;
        }

        Texture2D LoadPortraitTexture(string name)
        {
            using (var archive = ZipFile.OpenRead(filePath))
            {
                var textureEntry = archive.Entries.FirstOrDefault(x => x.Name == name);
                if (textureEntry != null)
                {
                    var stream = new MemoryStream();
                    textureEntry.Open().CopyTo(stream);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(stream.ToArray());
                    texture.wrapMode = TextureWrapMode.Clamp;
                    return texture;
                }
            }

            return null;
        }

        public static Texture2D GetSkinIconByID(string id)
        {
            AltSkinsPlugin.LogInfo($"Loading Skin Icon {id}");
            var skin = SkinManager.AllSkins.FirstOrDefault(x => x.skinId == id.Split('/')[1]);
            if (skin != null)
                return skin.LoadPortraitTexture(id.Split('/').Last());
            return null;
        }
    }
}
