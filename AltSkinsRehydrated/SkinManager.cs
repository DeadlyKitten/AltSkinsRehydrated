using AltSkinsRehydrated.Data;
using BepInEx;
using Nick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

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
            AltSkinsPlugin.Instance.doneLoading = true;
            AltSkinsPlugin.Instance.milliseconds = (int)watch.ElapsedMilliseconds;
            AltSkinsPlugin.LogInfo($"Loaded {AllSkins.Count} skins in {watch.ElapsedMilliseconds} ms.");
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

            AltSkinsPlugin.Instance.totalChar = files.Count();

            foreach (var file in files)
            {
                string loggedName = file.Substring(file.IndexOf("Skins"));
                try
                {
                    AltSkinsPlugin.LogInfo($"Loading skin: {loggedName}");
                    CustomSkin skin = new CustomSkin(file);
                    if (skin.metadata.version < 2) continue;
                    AllSkins.Add(skin);
                    AltSkinsPlugin.Instance.loadedChar++;

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
                    AltSkinsPlugin.LogError($"Failed to load skin: {loggedName}");
                    AltSkinsPlugin.LogError(e.ToString());
                }

            }
        }

        public static bool TryGetSkinIndexForChar(string charID, string skinID, out int index)
        {
            AltSkinsPlugin.LogInfo($"Searching for skin [{skinID}] in character [{charID}]");

            index = -1;
            var charMeta = charMetas.First(x => x.id == charID);

            if (charMeta)
            {
                for (var i = 0; i < charMeta.skins.Length; i++)
                    if (charMeta.skins[i].id == skinID) index = i;
            }

            AltSkinsPlugin.LogInfo($"index = {index}");

            return index >= 0;        
        }

        public static bool TryGetSkinID(byte charIndex, byte skinIndex, out string skinID)
        {
            skinID = string.Empty;
            if (charIndex >= 0 && charIndex < charMetas.Count)
            {
                var charMeta = charMetas[charIndex];
                if (skinIndex >= 0 && skinIndex < charMeta.skins.Length)
                {
                    skinID = charMeta.skins[skinIndex].id;
                }
            }

            return !string.IsNullOrEmpty(skinID);
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
