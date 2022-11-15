using HarmonyLib;
using Nick;
using System.Collections.Generic;

namespace AltSkinsRehydrated.HarmonyPatches.Online
{
    // Swaps custom skin back for local player
    [HarmonyPatch(typeof(OnlineMatch), "GetPlayerData")]
    class OnlineMatch_GetPlayerData
    {
        static void Postfix(ref List<GameSetup.PlayerData> __result)
        {
            for (int i = 0; i < __result.Count; i++)
            {
                var currentPlayer = __result[i];
                if (currentPlayer.extData.OnlineData.Local)
                {
                    Plugin.LogInfo($"Setting local player skin to {Plugin.onlineSkin}");
                    currentPlayer.skinIndex = Plugin.onlineSkin;
                    __result[i] = currentPlayer;
                    Plugin.LogInfo($"Set local player skin to {__result[i].skinIndex}");
                }
            }
        }
    }
}
