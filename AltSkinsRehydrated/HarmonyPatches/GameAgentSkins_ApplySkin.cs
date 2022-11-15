using HarmonyLib;
using Nick;
using SMU.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace AltSkinsRehydrated.HarmonyPatches
{
    [HarmonyPatch(typeof(GameAgentSkins), "ApplySkin")]
    class GameAgentSkins_ApplySkin
    {
        static void Prepare()
        {
            baseGameShaders = new Dictionary<string, Shader>();

            foreach (var shaderName in baseGameShaderNames)
            {
                var shader = Shader.Find(shaderName);

                if (!shader)
                {
                    Plugin.LogWarning($"Shader not found for '{shaderName}'");
                    continue;
                }

                baseGameShaders.Add(shaderName, Shader.Find(shaderName));
            }
        }

        static void Prefix(SkinData skin)
        {
            if (!skin) return;
            if (SkinManager.IsCustomSkin(skin.skinid))
            {
                var materialSwitches = skin.GetField<SkinData, SkinData.MaterialSwitch[]>("materialSwitches");
                if (materialSwitches == null) return;
                for (int i = 0; i < materialSwitches.Length; i++)
                {
                    var shaderName = materialSwitches[i].material?.shader?.name;
                    if (shaderName == null) continue;

                    if (baseGameShaders.TryGetValue(shaderName, out var shader))
                        materialSwitches[i].material.shader = shader;
                }
            }
        }

        static readonly List<string> baseGameShaderNames = new List<string>
        {
            "Nick/Characters/NickCharacters",
            "Nick/NickAdditive",
            "Shader_Master",
            "shader_master_2",
            "Shader_Fly_Trail",
            "Shader_rocket",
            "Hovl/Particles/SwordSlash",
            "Hovl/Particles/Blend_TwoSides"
        };

        static Dictionary<string, Shader> baseGameShaders;
    }
}
