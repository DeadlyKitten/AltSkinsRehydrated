using HarmonyLib;
using Nick;

namespace AltSkinsRehydrated.HarmonyPatches
{
    // Stops the game from breaking when generating an empty LoadedSkin
    // Since we're not generating scenes from scratch anymore this isn't really needed...
    // But it doesn't hurt anything so I'll leave it for now
    [HarmonyPatch(typeof(LoadedSkin), "OnEnable")]
    class LoadedSkin_OnEnable
    {
        static bool Prefix(ref LoadedSkin __instance)
        {
            if (__instance.skinId == null || __instance.skin == null)
                return false;
            else
                return true;
        }
    }
}
