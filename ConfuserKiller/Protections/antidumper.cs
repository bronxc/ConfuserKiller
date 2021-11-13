using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal static class antidumper {
    
        internal static int FindInstructionsNumber(this MethodDef method, OpCode opCode, object operand) {
            int num = 0;
            foreach (Instruction instruction in method.Body.Instructions) {
                bool flag = instruction.OpCode != opCode;
                if (!flag) {
                    bool flag2 = operand is int;
                    if (flag2) {
                        int ldcI4Value = instruction.GetLdcI4Value();
                        bool flag3 = ldcI4Value == (int)operand;
                        if (flag3) {
                            num++;
                        }
                    }
                    else {
                        bool flag4 = operand is string;
                        if (flag4) {
                            string text = instruction.Operand.ToString();
                            bool flag5 = text.Contains(operand.ToString());
                            if (flag5) {
                                num++;
                            }
                        }
                    }
                }
            }
            return num;
        }


    
        internal static bool Run(ModuleDefMD module) {
            IList<Instruction> instructions = module.GlobalType.FindStaticConstructor().Body.Instructions;
            foreach (Instruction instruction in instructions) {
                bool flag = instruction.OpCode != OpCodes.Call;
                if (!flag) {
                    MethodDef methodDef = instruction.Operand as MethodDef;
                    bool flag2 = methodDef == null;
                    if (!flag2) {
                        bool flag3 = !methodDef.DeclaringType.IsGlobalModuleType;
                        if (!flag3) {
                            bool flag4 = methodDef.Attributes != (MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.Static | MethodAttributes.HideBySig);
                            if (!flag4) {
                                bool flag5 = methodDef.CodeType > MethodImplAttributes.IL;
                                if (!flag5) {
                                    bool flag6 = methodDef.ReturnType.ElementType != ElementType.Void;
                                    if (!flag6) {
                                        bool flag7 = methodDef.FindInstructionsNumber(OpCodes.Call, "(System.Byte*,System.Int32,System.UInt32,System.UInt32&)") != 14;
                                        if (!flag7) {
                                            instruction.OpCode = OpCodes.Nop;
                                            instruction.Operand = null;
                                            return true;
                                        }
                                    }
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
