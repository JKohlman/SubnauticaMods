using SMLHelper.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static MoreQuickSlotsBepInEx.Config.CustomKeyboardShortcut;

namespace MoreQuickSlotsBepInEx.Config
{
    internal class SMLConfig : ModOptions
    {
        public SMLConfig() : base("More Quick Slots (BepInEx)")
        {
            OnChanged += Options_Changed;

            MoreQuickSlotsBepInEx.logger.LogInfo("BUILDING MOD OPTIONS");
            AddItem(ModToggleOption.Create("DAATQS", "Disable Auto-Bind", BepInExConfig.DAATQS.Value));
            AddItem(ModSliderOption.Create("Extra Slots", "Extra Slots", 0, 15, BepInExConfig.ExtraSlots.Value, 4, "{0}", 1));

            for (int i = 0; i < BepInExConfig.MAX_EXTRA_SLOTS; i++)
            {
                AddItem(ModChoiceOption<AllowedKeys>.Create("ExtraHotkey" + i.ToString().PadLeft(2, '0'), "Quickslot " + (i + 6) + " Hotkey", Enum.GetValues(typeof(AllowedKeys)).Cast<AllowedKeys>().ToArray(), (AllowedKeys)BepInExConfig.SlotHotkeys[i].MainKey));
                AddItem(CustomKeyBoardModifiersOption.Create($"ExtraHotkey {i.ToString().PadLeft(2, '0')}MOD", $"Quickslot {(i + 6)} Modifiers", BepInExConfig.SlotHotkeys[i]));
            }
            AddItem(ModButtonOption.Create("Button_1", "Factory Button", (ButtonClickedEventArgs e) => MoreQuickSlotsBepInEx.logger.LogInfo("Factory Button Clicked")));
            AddItem(ModBasicColorOption.Create("Color_1", "Test Color", Color.white));

            AddItem(ModColorOption.Create("Color_2", "Test Advanced Color", Color.white));
            AddItem(ModToggleOption.Create("After Color", "BLEH", false));
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
                case ChoiceChangedEventArgs<AllowedKeys> args:
                    if (args.Id.StartsWith("ExtraHotkey"))
                    {
                        int hotkeyNumber = int.Parse(args.Id.Substring(args.Id.Length - 2));
                        BepInExConfig.SlotHotkeys[hotkeyNumber]._MainKey.Value = args.Value;
                    }
                    break;
                case ColorChangedEventArgs args:
                    MoreQuickSlotsBepInEx.logger.LogInfo($"Changed color to {args.Value}");
                    break;
            }
        }
    }

    internal class CustomKeyBoardModifiersEventArgs : ConfigOptionEventArgs<string>
    {
        internal CustomKeyBoardModifiersEventArgs(string id, string values) : base(id, values) { }
    }

    internal class CustomKeyBoardModifiersOption : ModOption<string, CustomKeyBoardModifiersEventArgs>
    {
        private readonly CustomKeyboardShortcut _BackingField;

        public override void AddToPanel(uGUI_TabbedControlsPanel panel, int tabIndex)
        {
            Canvas canvas = new GameObject("Canvas", typeof(RectTransform)).AddComponent<Canvas>();

            GameObject gameObject = panel.AddItem(tabIndex, panel.toggleOptionPrefab, Label);
            Toggle toggle = gameObject.EnsureComponent<Toggle>();
            toggle.isOn = true;
            TextMeshProUGUI text = gameObject.EnsureComponent<TextMeshProUGUI>();
            text.text = "Test";

            //return componentInChildren1;
            //var modifiers = _BackingField.GetModifiers; // Fetch to consolidate string value with modifiers value
            //foreach (AllowedModifiers modifier in Enum.GetValues(typeof(AllowedModifiers)))
            //{
            //    UnityEngine.UI.Toggle toggle = panel.AddToggleOption(tabIndex, modifier.ToString(), modifiers[modifier],
            //        new UnityAction<bool>((bool value) =>
            //        {
            //            _BackingField.SetModifier(modifier, value);
            //        }));
            //}
            panel.AddItem(tabIndex, canvas.gameObject, Label);
        }

        private CustomKeyBoardModifiersOption(string id, string label, CustomKeyboardShortcut shortcut) : base(id, label, shortcut.ModifiersString)
        {
            _BackingField = shortcut;
        }

        public static CustomKeyBoardModifiersOption Create(string id, string label, CustomKeyboardShortcut shortcut)
        {
            return new CustomKeyBoardModifiersOption(id, label, shortcut);
        }

        //private class CustomKeyBoardModifiersOptionAdjust : ModOptionAdjust
        //{
        //    private const float spacing = 20f;

        //    public IEnumerator Start()
        //    {
        //        SetCaptionGameObject("Toggle/Caption");
        //        yield return null;

        //        Transform check = gameObject.transform.Find("Toggle/Background");

        //        if (CaptionWidth + spacing > check.localPosition.x)
        //        {
        //            check.localPosition = SetVec2x(check.localPosition, CaptionWidth + spacing);
        //        }

        //        Destroy(this);
        //    }
        //}
        public override Type AdjusterComponent => null;

    }
}
