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
            if (onlineMatchInfoList != null && onlineMatchInfoList.currUser != null)
            {
                if(OnlineManager.TryGetOnlineSkinIdForUser(onlineMatchInfoList.currUser.Id, out var skinID))
                {
                    if(SkinManager.TryGetSkinIndexForChar(onlineMatchInfoList.characterMetaData.id, skinID, out var index))
                    {
                        onlineMatchInfoList.skinIdx = index;
                    }
                }
            }
        }
    }
}
