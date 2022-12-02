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
                if (SkinManager.TryGetSkinID(__instance.character, __instance.skin, out var skinID))
                    OnlineManager.SetOnlineSkin(skinID);
                else
                    OnlineManager.SetOnlineSkin(string.Empty);

                __instance.skin = 0;
            }
        }
    }
}
