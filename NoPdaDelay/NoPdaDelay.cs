using BepInEx;
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
        private const string versionString = "1.0.0";

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource logger;

        public void Awake()
        {
            logger = Logger;
            harmony.PatchAll();
        }
    }
}
