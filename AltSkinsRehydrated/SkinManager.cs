using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nick;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using AltSkinsRehydrated.Data;

namespace AltSkinsRehydrated
{
    class SkinManager
    {
        public static List<CustomSkin> AllSkins = new List<CustomSkin>();
        public static string folder = Path.Combine(Paths.BepInExRootPath, "Skins");

        static List<CharacterMetaData> charMetas;

        internal static void Init()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            LoadSkins();
            // LoadLegacySkins();

            watch.Stop();
            Plugin.Instance.doneLoading = true;
            Plugin.Instance.milliseconds = (int)watch.ElapsedMilliseconds;
            Plugin.LogInfo($"Loaded {AllSkins.Count} skins in {watch.ElapsedMilliseconds} ms.");
        }

        static void LoadSkins()
        {
            if (charMetas == null)
            {
                var gameMeta = Resources.FindObjectsOfTypeAll<GameMetaData>().First();
                charMetas = gameMeta.characterMetas.ToList();
            }

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string[] files = Directory.GetFiles(folder, "*.nickskin", SearchOption.AllDirectories);

            Plugin.Instance.totalChar = files.Count();

            foreach (var file in files)
            {
                string loggedName = file.Substring(file.IndexOf("Skins"));
                try
                {
                    Plugin.LogInfo($"Loading skin: {loggedName}");
                    CustomSkin skin = new CustomSkin(file);
                    if (skin.metadata.version < 2) continue;
                    AllSkins.Add(skin);
                    Plugin.Instance.loadedChar++;

                    var newSkinMeta = new CharacterMetaData.CharacterSkinMetaData[]
                    {
                        new CharacterMetaData.CharacterSkinMetaData()
                        {
                            id = skin.skinId,
                            locName = skin.name,
                            resPortrait = skin.largeIconId,
                            resMediumPortrait = skin.medIconId,
                            resMiniPortrait = skin.smallIconId,
                            unlockSkin = string.Empty,
                        }
                    };

                    var currentCharacterMeta = charMetas.First(x => x.id == skin.characterId);
                    currentCharacterMeta.skins = currentCharacterMeta.skins.Concat(newSkinMeta).ToArray();
                }
                catch (Exception e)
                {
                    Plugin.LogError($"Failed to load skin: {loggedName}");
                    Plugin.LogError(e.ToString());
                }

            }
        }

        public static bool TryGetSkinById(string id, out CustomSkin skin)
        { 
            skin = null;
            if (IsCustomSkin(id))
            {
                skin = GetSkinById(id);
                return true;
            }
            return false;
        }
        
        public static CustomSkin GetSkinById(string id)
        {
            return AllSkins.Where(x => x.skinId == id).FirstOrDefault();
        }

        public static bool IsCustomSkin(string id)
        {
            return AllSkins.Any(x => x.skinId == id);
        }
    }
}
