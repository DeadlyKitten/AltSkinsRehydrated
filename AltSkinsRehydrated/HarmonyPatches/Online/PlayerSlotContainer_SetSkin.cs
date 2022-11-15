using System;
using System.Collections.Generic;
using System.Text;
using Nick;
using HarmonyLib;

namespace AltSkinsRehydrated.HarmonyPatches.Online
{
    // honestly this isn't really used anymore
    [HarmonyPatch(typeof(PlayerSlotContainer), "SetSkin")]
    class PlayerSlotContainer_SetSkin
    {
        static void Postfix(PlayerSlotContainer __instance, ref bool ___isOnline)
        {
            if (__instance.playerSlotIndex == 0 && ___isOnline)
            {
                //Plugin.onlineSkin = (byte)__instance.PlayerSetup.skin;
                Plugin.onlineSkinId = __instance.CurrentCharMeta.skins[__instance.PlayerSetup.skin].id;

                Plugin.LogInfo($"Skin Index: {Plugin.onlineSkin}");
                Plugin.LogInfo($"Skin ID: {Plugin.onlineSkinId}");
            }
        }
    }
}
