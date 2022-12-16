using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace PDAInstaClose.Patches
{
    [HarmonyPatch(typeof(PDA), "Open")]
    public static class PDAOpenPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "sequence"))
                .Advance(1)
                .SetOperandAndAdvance(0.0f)
                .InstructionEnumeration();
        }
    }

    [HarmonyPatch(typeof(PDA), "Close")]
    public static class PDAClosePatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
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
