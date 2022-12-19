using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoPdaDelay.Patches
{
    [HarmonyPatch(typeof(PDA))]
    public static class PDAOpenPatch
    {
        [HarmonyPatch("Open")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> OpenTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "sequence"))
                .Advance(1)
                .SetOperandAndAdvance(0.0f)
                .InstructionEnumeration();
        }

        [HarmonyPatch("Close")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> CloseTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "sequence"))
                .Advance(1)
                .SetOperandAndAdvance(0.0f)
                .InstructionEnumeration();
        }
    }
}
