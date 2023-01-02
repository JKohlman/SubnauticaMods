using SMLHelper.Options;
using System;
using static MoreQuickSlotsBepInEx.Config.CustomKeyboardShortcut;

namespace MoreQuickSlotsBepInEx.Config
{
    internal class SMLConfig : ModOptions
    {
        public SMLConfig() : base("More Quick Slots (BepInEx)")
        {
            OnChanged += Options_Changed;
        }

        public override void BuildModOptions()
        {
            AddOption(ModToggleOption.Factory("DAATQS", "Disable Auto-Bind", BepInExConfig.DAATQS.Value));
            AddOption(ModSliderOption.Factory("Extra Slots", "Extra Slots", 0, 15, BepInExConfig.ExtraSlots.Value, 4, "{0}", 1));

            for (int i = 0; i < BepInExConfig.MAX_EXTRA_SLOTS; i++)
            {
                AddOption(ModChoiceOption.Factory("ExtraHotkey" + i.ToString().PadLeft(2, '0'), "Quickslot " + (i + 6) + " Hotkey", (AllowedKeys)BepInExConfig.SlotHotkeys[i].MainKey));
            }
        }

        private void Options_Changed(object sender, EventArgs e)
        {
            switch (e)
            {
                case ToggleChangedEventArgs args:
                    switch (args.Id)
                    {
                        case "DAATQS":
                            BepInExConfig.DAATQS.Value = args.Value;
                            break;
                    }
                    break;
                case SliderChangedEventArgs args:
                    switch (args.Id)
                    {
                        case "Extra Slots":
                            BepInExConfig.ExtraSlots.Value = (int)args.Value;
                            break;
                    }
                    break;
                case ChoiceChangedEventArgs args:
                    if (args.Id.StartsWith("ExtraHotkey"))
                    {
                        int hotkeyNumber = int.Parse(args.Id.Substring(args.Id.Length - 2));
                        BepInExConfig.SlotHotkeys[hotkeyNumber]._MainKey.Value = (AllowedKeys)Enum.Parse(typeof(AllowedKeys), args.Value.Value);
                    }
                    break;
            }
        }
    }

    //internal class CustomOption<T> : ModOption
    //{
    //    public T Value { get; }
    //    public T DefaultValue { get; }

    //    public override void AddToPanel(uGUI_TabbedControlsPanel panel, int tabIndex)
    //    {
    //        var option = panel.AddCustomOption(); // TODO: Replace with actual code
    //        OptionGameObject = option.transform.parent.gameObject;
    //        base.AddToPanel(panel, tabIndex);
    //    }

    //    public CustomOption(string id, string label, T value) : base(label, id)
    //    {
    //        this.Value = value;
    //    }


    //    public override Type AdjusterComponent => typeof(CustomOption<T>.CustomOptionAdjust);

    //    private class CustomOptionAdjust : ModOption.ModOptionAdjust
    //    {

    //    }
    //}
}
