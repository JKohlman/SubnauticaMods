using SMLHelper.V2.Options;
using System;
using static MoreQuickSlotsBepInEx.Config.CustomKeyboardShortcut;

namespace MoreQuickSlotsBepInEx.Config
{
    internal class SMLConfig : ModOptions
    {
        public SMLConfig() : base("More Quick Slots (BepInEx)")
        {
            ToggleChanged += Options_ToggleChanged;
            SliderChanged += Options_SliderChanged;
            ChoiceChanged += Options_ChoiceChanged;
        }

        public override void BuildModOptions()
        {
            AddToggleOption("DAATQS", "Disable Auto-Bind", BepInExConfig.DAATQS.Value);
            AddSliderOption("Extra Slots", "Extra Slots", 0, 15, BepInExConfig.ExtraSlots.Value, 4, "{0}", 1);

            for (int i = 0; i < BepInExConfig.MAX_EXTRA_SLOTS; i++)
            {
                AddChoiceOption("ExtraHotkey" + i.ToString().PadLeft(2, '0'), "Quickslot " + (i + 6) + " Hotkey", (AllowedKeys)BepInExConfig.SlotHotkeys[i].MainKey);
            }
        }

        private void Options_ToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            switch (e.Id)
            {
                case "DAATQS":
                    BepInExConfig.DAATQS.Value = e.Value;
                    break;
            }
        }

        private void Options_SliderChanged(object sender, SliderChangedEventArgs e)
        {
            switch (e.Id)
            {
                case "Extra Slots":
                    BepInExConfig.ExtraSlots.Value = (int)e.Value;
                    break;
            }
        }

        private void Options_ChoiceChanged(object sender, ChoiceChangedEventArgs e)
        {
            if (e.Id.StartsWith("ExtraHotkey"))
            {
                int hotkeyNumber = int.Parse(e.Id.Substring(e.Id.Length - 2));
                BepInExConfig.SlotHotkeys[hotkeyNumber]._MainKey.Value = (AllowedKeys)Enum.Parse(typeof(AllowedKeys), e.Value);
            }
        }
    }
}
