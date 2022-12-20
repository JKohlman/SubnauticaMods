using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MoreQuickSlotsBepInEx.Patches;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreQuickSlotsBepInEx
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    [BepInProcess("Subnautica.exe")]
    public class MoreQuickSlotsBepInEx : BaseUnityPlugin
    {
        private const string myGUID = "com.essence.MoreQuickSlotsBepInEx";
        private const string pluginName = "More Quick Slots (BepInEx)";
        private const string versionString = "1.0.0";

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource logger;

        public const int MAX_EXTRA_SLOTS = 15;

        private readonly KeyCode[] defaultHotkeys = { KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, };

        public static ConfigEntry<int> CfgExtraSlots;
        public static List<ConfigEntry<KeyboardShortcut>> CfgSlotHotkeys = new List<ConfigEntry<KeyboardShortcut>>();

        public void Awake()
        {
            createConfig();
            logger = Logger;
            harmony.PatchAll();
        }

        private void createConfig()
        {
            CfgExtraSlots = Config.Bind(
                "General", 
                "Extra Slots", 
                4, 
                new ConfigDescription(
                    "How many extra slots to add", 
                    new AcceptableValueRange<int>(0, MAX_EXTRA_SLOTS)
                )
            );
            CfgExtraSlots.SettingChanged += (object sender, EventArgs e) => { QuickSlots_Patch.ReDrawSlots(); };
            for (int i = 0; i < MAX_EXTRA_SLOTS; i++)
            {
                CfgSlotHotkeys.Add(Config.Bind(
                    "Hotkeys",
                    "Quickslot " + (i + 6).ToString() + " Hotkey",
                    new KeyboardShortcut((i < defaultHotkeys.Length) ? defaultHotkeys[i] : KeyCode.None),
                    new ConfigDescription(
                        "Hotkey for quickslot " + (i + 6).ToString(),
                        null,
                        new ConfigurationManagerAttributes { Order = (MAX_EXTRA_SLOTS + 1) - i }
                    )
                ));
            }
        }

        private void Update()
        {
            pollHotkeys();
        }

        private static void pollHotkeys()
        {
            if (Inventory.main == null)
                return;

            for (int i = 0; i < CfgExtraSlots.Value; i++)
            {
                if (CfgSlotHotkeys[i].Value.IsDown())
                {
                    Inventory.main.quickSlots.SlotKeyDown(i + 5);
                }
            }
        }
    }
}
