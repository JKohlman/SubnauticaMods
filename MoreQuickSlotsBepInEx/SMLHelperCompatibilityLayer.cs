using MoreQuickSlotsBepInEx.Config;
using SMLHelper.V2.Handlers;

namespace MoreQuickSlotsBepInEx
{
    internal static class SMLHelperCompatibilityLayer
    {
        internal const string SMLHelper_GUID = "com.ahk1221.smlhelper";

        internal static bool Loaded => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(SMLHelper_GUID);
        internal static void Initialize()
        {
            if (Loaded)
            {
                MoreQuickSlotsBepInEx.logger.LogInfo("Loading SML Config");
                SMLPresentInitialize();
                MoreQuickSlotsBepInEx.logger.LogInfo("Finished Loading SML Config");
            }
        }

        private static void SMLPresentInitialize()
        {
            OptionsPanelHandler.Main.RegisterModOptions(new SMLConfig());
        }
    }
}
