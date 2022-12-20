using UnityEngine;

namespace MoreQuickSlotsBepInEx.Objects
{
    public class CustomInput : MonoBehaviour
    {
        private void Update()
        {
            for (int i = 0; i < MoreQuickSlotsBepInEx.MAX_EXTRA_SLOTS; i++)
            {
                if (MoreQuickSlotsBepInEx.CfgSlotHotkeys[i].Value.IsDown())
                {
                    Inventory.main.quickSlots.SlotKeyDown(i + 5);
                }
            }
        }
    }
}
