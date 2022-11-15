using AltSkinsRehydrated.Data;
using HarmonyLib;
using Nick;
using System;
using System.Reflection;
using UnityEngine;

// I hate this patch with every fiber of my being...
namespace AltSkinsRehydrated.HarmonyPatches
{
    [HarmonyPatch]
    class ResourceTextureHandles_TexState_ctor
    {
        static MethodBase TargetMethod()
        {
            return typeof(ResourceTextureHandles)
                .GetNestedType("TexState", BindingFlags.NonPublic)
                .GetConstructor(new Type[] { typeof(string) });
        }

        static bool Prefix(ref string ___resPath, ref Texture2D ___tex, ref int ___3, string resourcePath)
        {
            Plugin.LogInfo($"Resource Path: {resourcePath}");

            if (resourcePath.StartsWith("CUSTOM_"))
            {
                ___resPath = resourcePath;
                ___tex = CustomSkin.GetSkinIconByID(resourcePath);
                if (!___tex) return true;
                ___3 = 2; // set loaded state to Loaded
                return false;
            }
            return true;
        }
    }
}
