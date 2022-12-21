using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace NoPdaDelay
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    [BepInProcess("Subnautica.exe")]
    public class NoPdaDelay : BaseUnityPlugin
    {
        private const string myGUID = "com.essence.NoPdaDelay";
        private const string pluginName = "No PDA Delay";
        private const string versionString = "1.1.0";

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource logger;

        public static ConfigEntry<float> CfgOpenDelay { get; set; }
        public static ConfigEntry<float> CfgCloseDelay { get; set; }

        public void Awake()
        {
            createConfig();
            logger = Logger;
            harmony.PatchAll();
        }

        private void createConfig()
        {
            CfgOpenDelay = Config.Bind("Delays", "Open Delay", 0.0f, "How long to delay while opening the PDA");
            CfgCloseDelay = Config.Bind("Delays", "Close Delay", 0.0f, "How long to delay while closing the PDA");
        }
    }
}
