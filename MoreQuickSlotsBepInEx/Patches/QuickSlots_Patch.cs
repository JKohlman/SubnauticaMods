using HarmonyLib;
using MoreQuickSlotsBepInEx.Config;
using System;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    public class QuickSlots_Patch
    {
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(GameObject), typeof(Transform), typeof(Transform), typeof(Inventory), typeof(Transform), typeof(int) })]
        [HarmonyPrefix]
        static void Ctor_Prefix(ref int slotCount)
        {
            slotCount += PluginConfig.ExtraSlots.Value;
        }

        [HarmonyPatch(nameof(QuickSlots.BindToEmpty))]
        [HarmonyPrefix]
        static bool BindToEmpty_Prefix(ref int __result)
        {
            if (PluginConfig.DAATQS.Value)
            {
                __result = -1;
                return false;
            }
            return true;
        }

        internal static void SetupSlotNames()
        {
            string[] mySlotNames = new string[PluginConfig.MAX_EXTRA_SLOTS + QuickSlots.slotNames.Length];
            for (int i = 0; i < mySlotNames.Length; i++) { mySlotNames[i] = "QuickSlot" + i.ToString(); }

            Traverse.Create<QuickSlots>().Field("slotNames").SetValue(mySlotNames);
        }

        internal static void ReDrawSlots()
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
