using SMLHelper.Json;
using SMLHelper.Options;
using SMLHelper.Options.Attributes;
using System;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Config
{
    [Menu("More Quick Slots (BepInEx) Extended")]
    public class SMLConfigAttributed : ConfigFile
    {
        [Button(Id = "Button_2", Label = "Attribute Button", Tooltip = "Attribute Tooltip")]
        public void MyCallback(ButtonClickedEventArgs e)
        {
            MoreQuickSlotsBepInEx.logger.LogInfo($"{e.Id} (Attribute Button) clicked");
        }

        [ColorPicker(Id="Color_Id", Label = "My Color Attribute")]
        [OnChange(nameof(MyGenericOnChangeEvent))]
        public Color myColor;

        [Toggle("123")]
        [OnChange(nameof(MyGenericOnChangeEvent))]
        public bool MyCheckbox = true;

        public static void MyGenericOnChangeEvent(object sender, EventArgs e)
        {
            MoreQuickSlotsBepInEx.logger.LogDebug($"GOT CALLBACK IN ONCHANGE");
        }
    }
}
