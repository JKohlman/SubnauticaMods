using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    public class QuickSlots_Patch
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

        public static void ReDrawSlots()
        {
            Inventory inv = Inventory.main;
            if (inv == null)
                return;

            // Save slot bindings and selected slot to reapply (if possible)
            InventoryItem[] oldBinding = inv.quickSlots.binding;
            int oldDesiredSlot = inv.quickSlots.desiredSlot;

            inv.quickSlots.DeselectImmediate();
            // Rely on Prefix to handle adding more slots, next line mimics vanilla SN
            inv.quickSlots = new QuickSlots(inv.gameObject, inv.toolSocket, inv.cameraSocket, inv, inv.GetComponent<Player>().rightHandSlot, 5);

            // Reapply slot bindings and selected
            for (int i = 0; (i < oldBinding.Length) && (i < inv.quickSlots.binding.Length); i++)
                inv.quickSlots.binding[i] = oldBinding[i];

            if (oldDesiredSlot <= inv.quickSlots.slotCount)
                inv.quickSlots.Select(oldDesiredSlot);
        }
    }
}
