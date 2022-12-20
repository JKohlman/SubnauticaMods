using HarmonyLib;
using MoreQuickSlotsBepInEx.Objects;

namespace MoreQuickSlotsBepInEx.Patches
{
    [HarmonyPatch(typeof(Player))]
    internal class Player_Patch
    {
        [HarmonyPatch("Awake")]
        static void Postfix(Player __instance)
        {
            __instance.gameObject.EnsureComponent<CustomInput>();
        }
    }
}
