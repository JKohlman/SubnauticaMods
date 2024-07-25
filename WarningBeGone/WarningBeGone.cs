using BepInEx;

namespace WarningBeGone
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    #if SN
    [BepInProcess("Subnautica.exe")]
    #elif BZ
    [BepInProcess("SubnauticaZero.exe")]
    #endif
    public class WarningBeGone : BaseUnityPlugin
    {
        private const string myGUID = "com.essence.WarningBeGone";
        private const string pluginName = "Warning Be Gone";
        private const string versionString = "1.0.0";

        private void Awake() => FlashingLightsDisclaimer.isFirstRun = false;
    }
}