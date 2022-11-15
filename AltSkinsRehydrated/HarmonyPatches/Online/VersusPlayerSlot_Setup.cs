using HarmonyLib;
using Nick;
using System;

namespace AltSkinsRehydrated.HarmonyPatches.Online
{
    // This fixes the skin in the loading screen
    [HarmonyPatch(typeof(VersusPlayerSlot), "Setup", new Type[] { typeof(OnlineMatchInfoListItem)})]
    class VersusPlayerSlot_Setup
    {
        static void Prefix(ref OnlineMatchInfoListItem onlineMatchInfoList)
        {
            if (onlineMatchInfoList != null && onlineMatchInfoList.currUser != null && onlineMatchInfoList.currUser.IsLocal)
            {
                onlineMatchInfoList.skinIdx = Plugin.onlineSkin;
            }
        }
    }
}
