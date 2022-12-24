using HarmonyLib;
using MoreQuickSlotsBepInEx.Config;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(TooltipFactory))]
    public class TooltipFactory_Patch
    {
        [HarmonyPatch(nameof(TooltipFactory.RefreshActionStrings))]
        static void Postfix()
        {
            int highSlot = 5 + PluginConfig.ExtraSlots.Value;
            TooltipFactory.stringKeyRange15 = "1-" + ((highSlot > 9) ? "9+" : highSlot.ToString());
        }
    }
}
