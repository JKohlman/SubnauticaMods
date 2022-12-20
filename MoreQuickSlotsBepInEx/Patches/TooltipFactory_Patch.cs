using HarmonyLib;
using System.Reflection;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(TooltipFactory))]
    internal class TooltipFactory_Patch
    {
        [HarmonyPatch("RefreshActionStrings")]
        static void Postfix()
        {
            string highNum = ((5 + MoreQuickSlotsBepInEx.CfgExtraSlots.Value) % 10).ToString();
            typeof(TooltipFactory).GetField("stringKeyRange15", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, "1-"+highNum);
        }
    }
}
