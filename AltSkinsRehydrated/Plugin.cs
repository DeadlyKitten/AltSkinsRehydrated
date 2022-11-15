using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nick;
using SMU.Reflection;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AltSkinsRehydrated
{
    [BepInPlugin("com.steven.nasb.altskins-rehydrated", "AltSkins Rehydrated", "0.0.1")]
    [BepInIncompatibility("legoandmars-altskins")]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;

        public int totalChar = 0;
        public int loadedChar = 0;
        public int milliseconds = 0;
        public bool doneLoading = false;

        public static bool IsOnline => runner.GetField<GameRunner, GameRunner.State>("state") != GameRunner.State.Inactive;
        public static byte onlineSkin = 1;
        public static string onlineSkinId = string.Empty;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;
            Instance.gameObject.name = "AltSkins Rehydrated";

            LogInfo("AltSkins Rehydrated starting up...");

            var harmony = new Harmony("com.steven.nasb.altskins-rehydrated");
            harmony.PatchAll();

            SkinManager.Init();
            StartCoroutine(FindOnlineGameRunner());
        }

        static GameRunner runner;

        IEnumerator FindOnlineGameRunner()
        {
            yield return new WaitUntil(() => runner = Resources.FindObjectsOfTypeAll<OnlineGameRunner>().FirstOrDefault());
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
