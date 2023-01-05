using SMLHelper.Json;
using SMLHelper.Options;
using SMLHelper.Options.Attributes;
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
        public Color myColor;
    }
}
