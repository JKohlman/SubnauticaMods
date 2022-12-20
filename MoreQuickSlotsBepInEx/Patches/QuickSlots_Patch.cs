using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    internal class QuickSlots_Patch
    {
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(GameObject), typeof(Transform), typeof(Transform), typeof(Inventory), typeof(Transform), typeof(int) })]
        static void Prefix(ref int slotCount)
        {
            slotCount += MoreQuickSlotsBepInEx.CfgExtraSlots.Value;

            string[] mySlotNames = new string[slotCount];
            for (int i = 0; i < slotCount; i++) { mySlotNames[i] = "QuickSlot" + i.ToString(); }

            typeof(QuickSlots).GetField("slotNames", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, mySlotNames);
        }
    }
}
