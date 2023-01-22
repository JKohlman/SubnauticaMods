using SMLHelper.Json;
using SMLHelper.Options;
using SMLHelper.Options.Attributes;
using System;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Config
{
    [Menu("More Quick Slots (BepInEx) Extended")]
    internal class SMLConfigAttributed : ConfigFile
    {
        [Button(Id = "Button_2", Label = "Attribute Button", Tooltip = "Attribute Tooltip")]
        public void MyCallback(ButtonClickedEventArgs e)
        {
            MoreQuickSlotsBepInEx.logger.LogInfo($"{e.Id} (Attribute Button) clicked");
        }

        [ColorPicker(Id="Color_Id", Label = "Color Attribute")]
        [OnChange(nameof(MyGenericOnChangeEvent))]
        public Color myColor;

        [ColorPicker(Id = "Color_Id_2", Label = "Color Attribute 2", Advanced = true)]
        [OnChange(nameof(MyGenericOnChangeEvent))]
        public Color myColor2 = Color.white;

        [Toggle("123")]
        [OnChange(nameof(MyGenericOnChangeEvent))]
        public bool MyCheckbox = true;

        public static void MyGenericOnChangeEvent(object sender, EventArgs e)
        {
            MoreQuickSlotsBepInEx.logger.LogDebug($"GOT CALLBACK IN ONCHANGE");
        }
    }
}
