using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoPdaDelay.Patches
{
    [HarmonyPatch(typeof(PDA))]
    public static class PDAOpenPatch
    {
        [HarmonyPatch(nameof(PDA.Open))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> OpenTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "sequence"))
                .Advance(1)
                .SetInstruction(
                    Transpilers.EmitDelegate<Func<float>>(() => NoPdaDelay.CfgOpenDelay.Value)
                )
                .InstructionEnumeration();
        }

        [HarmonyPatch(nameof(PDA.Close))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> CloseTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name == "sequence"))
                .Advance(1)
                .SetInstruction(
                    Transpilers.EmitDelegate<Func<float>>(() => NoPdaDelay.CfgCloseDelay.Value)
                )
                .InstructionEnumeration();
        }
    }
}
