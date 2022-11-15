using HarmonyLib;
using Nick;

namespace AltSkinsRehydrated.HarmonyPatches.Online
{
    // Prevents the mod from breaking vanilla games
    [HarmonyPatch(typeof(OnlineLobby.CharacterSetup), "Write")]
    class OnlineLobby_CharacterSetup_Write
    {
        static void Prefix(ref OnlineLobby.CharacterSetup __instance)
        {
            if (__instance.skin > 1)
            {
                Plugin.onlineSkin = __instance.skin;
                __instance.skin = 0;
            }
        }
    }
}
