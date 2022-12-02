using AltSkinsRehydrated.Data;
using HarmonyLib;
using Nick;
using System.Collections.Generic;

namespace AltSkinsRehydrated.HarmonyPatches
{
    [HarmonyPatch(typeof(AgentLoading), "RequestLoad")]
    class AgentLoading_RequestLoad
    {
        static void Prefix(ref AgentLoading.LoadRequest req, ref Dictionary<string, AgentLoading.LoadState> ___loadStates, ref int ___loadCounter)
        {
            AltSkinsPlugin.LogInfo($"Agent Loading Request: {req.Id}");

            var skin = SkinManager.GetSkinById(req.Id);
            if (skin != null)
            {
                AltSkinsPlugin.LogInfo($"Custom skin detected! Skin ID: {req.Id}");

                var loadstate = new AgentLoading.LoadState() { phase = AgentLoading.LoadPhase.Loading, op = new UnityEngine.AsyncOperation() };

                if (!___loadStates.ContainsKey(req.Id))
                {
                    ___loadStates.Add(req.Id, loadstate);
                }

                skin.LoadAssetBundle(loadstate);

                ___loadCounter++;
            }
        }
    }
}
