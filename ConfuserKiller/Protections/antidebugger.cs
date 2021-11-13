using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal static class antidebugger {
      
        internal static bool Run(ModuleDefMD krawk) {
            IList<Instruction> instructions = krawk.GlobalType.FindStaticConstructor().Body.Instructions;
            foreach (Instruction instruction in instructions) {
                bool flag = instruction.OpCode != OpCodes.Call;
                bool flag2 = !flag;
                if (flag2) {
                    MethodDef methodDef = instruction.Operand as MethodDef;
                    bool flag3 = methodDef == null;
                    bool flag4 = !flag3;
                    if (flag4) {
                        bool flag5 = methodDef.FindInstructionsNumber(OpCodes.Ldstr, "ENABLE_PROFILING") != 1;
                        bool flag6 = !flag5;
                        if (flag6) {
                            bool flag7 = methodDef.FindInstructionsNumber(OpCodes.Ldstr, "GetEnvironmentVariable") != 1;
                            bool flag8 = !flag7;
                            if (flag8) {
                                bool flag9 = methodDef.FindInstructionsNumber(OpCodes.Call, "System.Environment::FailFast(System.String)") != 1;
                                bool flag10 = !flag9;
                                if (flag10) {
                                    instruction.OpCode = OpCodes.Nop;
                                    instruction.Operand = null;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
