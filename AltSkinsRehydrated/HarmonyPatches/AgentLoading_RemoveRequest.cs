using HarmonyLib;
using Nick;

namespace AltSkinsRehydrated.HarmonyPatches
{
    [HarmonyPatch(typeof(AgentLoading), "RemoveRequest")]
    class AgentLoading_RemoveRequest
    {
        static void Prefix(ref AgentLoading.LoadRequest req)
        {
            AltSkinsPlugin.LogInfo($"Agent Unloading Request: {req.Id}");

            var skin = SkinManager.GetSkinById(req.Id);
            if (skin != null)
            {
                skin.UnloadAssetBundle();
            }
        }
    }
}
