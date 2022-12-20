using HarmonyLib;
using System.Reflection;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(TooltipFactory))]
    internal class TooltipFactory_Patch
    {
        [HarmonyPatch(nameof(TooltipFactory.RefreshActionStrings))]
        static void Postfix()
        {
            int highSlot = 5 + MoreQuickSlotsBepInEx.CfgExtraSlots.Value;
            TooltipFactory.stringKeyRange15 = "1-" + ((highSlot > 9) ? "9+" : highSlot.ToString());
        }
    }
}
