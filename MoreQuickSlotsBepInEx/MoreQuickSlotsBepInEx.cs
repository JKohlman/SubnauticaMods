using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MoreQuickSlotsBepInEx.Config;
using MoreQuickSlotsBepInEx.Patches;

namespace MoreQuickSlotsBepInEx
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    [BepInProcess("Subnautica.exe")]
    public class MoreQuickSlotsBepInEx : BaseUnityPlugin
    {
        #region BEPINEX
        private const string myGUID = "com.essence.MoreQuickSlotsBepInEx";
        private const string pluginName = "More Quick Slots (BepInEx)";
        private const string versionString = "1.2.0";

        private static readonly Harmony harmony = new Harmony(myGUID);
        internal static ManualLogSource logger;
        #endregion

        public void Awake()
        {
            PluginConfig.Initialize(Config);
            logger = Logger;
            QuickSlots_Patch.SetupSlotNames(); // Fix those stupid slotnames issues
            harmony.PatchAll();
        }

        private void Update()
        {
            pollHotkeys();
        }

        private static void pollHotkeys()
        {
            if (Inventory.main == null)
                return;

            for (int i = 0; i < PluginConfig.ExtraSlots.Value; i++)
            {
                if (PluginConfig.SlotHotkeys[i].IsDown())
                    Inventory.main.quickSlots.SlotKeyDown(i + 5);
            }
        }
    }
}
