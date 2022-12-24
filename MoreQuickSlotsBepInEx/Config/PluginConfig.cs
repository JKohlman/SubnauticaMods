using BepInEx.Configuration;
using static MoreQuickSlotsBepInEx.Config.CustomKeyboardShortcut;
using System.Collections.Generic;
using System;
using MoreQuickSlotsBepInEx.Patches;

namespace MoreQuickSlotsBepInEx.Config
{
    public static class PluginConfig
    {
        public static readonly int MAX_EXTRA_SLOTS = 15;

        private static readonly AllowedKeys[] defaultHotkeys = { AllowedKeys.Alpha6, AllowedKeys.Alpha7, AllowedKeys.Alpha8, AllowedKeys.Alpha9, AllowedKeys.Alpha0, };

        public static ConfigEntry<bool> DAATQS;
        public static ConfigEntry<int> ExtraSlots;
        public static List<CustomKeyboardShortcut> SlotHotkeys = new List<CustomKeyboardShortcut>();

        internal static void Initialize(ConfigFile cfg)
        {
            DAATQS = cfg.Bind(
                "General",
                "Disable Auto-Bind",
                false,
                "Disable automatically adding picked up items to quickslots"
            );
            ExtraSlots = cfg.Bind(
                "General",
                "Extra Slots",
                4,
                new ConfigDescription(
                    "How many extra slots to add",
                    new AcceptableValueRange<int>(0, MAX_EXTRA_SLOTS)
                )
            );
            ExtraSlots.SettingChanged += (object sender, EventArgs e) => { QuickSlots_Patch.ReDrawSlots(); };
            for (int i = 0; i < MAX_EXTRA_SLOTS; i++)
            {
                SlotHotkeys.Add(new CustomKeyboardShortcut(
                    cfg.Bind(
                        "Hotkeys",
                        "Quickslot " + (i + 6).ToString() + " Hotkey",
                        (i < defaultHotkeys.Length) ? defaultHotkeys[i] : AllowedKeys.None,
                        new ConfigDescription(
                            "Hotkey for quickslot " + (i + 6).ToString(),
                            null,
                            new ConfigurationManagerAttributes
                            {
                                Order = (MAX_EXTRA_SLOTS + 1) - i
                            }
                        )
                    ),
                    cfg.Bind(
                        "Hotkeys",
                        "Quickslot " + (i + 6).ToString() + " Modifiers",
                        "",
                        new ConfigDescription(
                            "Modifiers for quickslot " + (i + 6).ToString(),
                            null,
                            new ConfigurationManagerAttributes
                            {
                                Order = (MAX_EXTRA_SLOTS + 1) - i,
                                CustomDrawer = new CustomDrawers().HotkeyModifiersDrawer
                            }
                        )
                    )
                ));
            }
        }
    }
}
