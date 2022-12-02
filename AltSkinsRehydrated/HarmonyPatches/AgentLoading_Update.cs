using HarmonyLib;
using Nick;
using System.Collections.Generic;
using UnityEngine;

namespace AltSkinsRehydrated.HarmonyPatches
{
    // Loading tends to get stuck, this stops that from happening
    [HarmonyPatch(typeof(AgentLoading), "Update")]
    class AgentLoading_Update
    {
        static void Prefix(AgentLoading __instance, ref List<AgentLoading.KeepLoadedState> ___keepLoaded, ref int ___loadCounter, ref Dictionary<string, AgentLoading.LoadState> ___loadStates)
        {
            if (___keepLoaded.Count <= 0) return;

            foreach(var keepLoadedState in ___keepLoaded)
            {
                if (SkinManager.TryGetSkinById(keepLoadedState.id, out var skin))
                {
                    if (skin.loading) return;
                }
            }

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 0.5f)
            {
                ___loadCounter = (___loadCounter + 1) % ___keepLoaded.Count;
                elapsedTime = 0f;
            }
        }

        static float elapsedTime = 0f;
    }
}
