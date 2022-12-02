using HarmonyLib;
using Nick;
using System.Collections.Generic;

namespace AltSkinsRehydrated.HarmonyPatches.Online
{
    // Set custom skins for all players
    [HarmonyPatch(typeof(OnlineMatch), "GetPlayerData")]
    class OnlineMatch_GetPlayerData
    {
        static void Postfix(OnlineMatch __instance, ref List<GameSetup.PlayerData> __result)
        {
            for (int i = 0; i < __result.Count; i++)
            {
                var currentPlayer = __result[i];

                var user = __instance.Users[i].LobbyUser.Id;
                if (OnlineManager.TryGetOnlineSkinIdForUser(user, out var skinID))
                {
                    if (SkinManager.TryGetSkinIndexForChar(currentPlayer.charMeta.id, skinID, out var index))
                    {
                        currentPlayer.skinIndex = index;
                        __result[i] = currentPlayer;
                    }
                }
            }
        }
    }
}
