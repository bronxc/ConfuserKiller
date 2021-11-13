using System;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class Constants {
 
        public static int constants() {
            int num = 0;
            Module manifestModule = Program.asm.ManifestModule;
            foreach (TypeDef typeDef in Program.module.GetTypes()) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                            bool flag3 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("tring>") && methodDef.Body.Instructions[i].Operand is MethodSpec;
                            bool flag4 = flag3;
                            if (flag4) {
                                bool flag5 = methodDef.Body.Instructions[i - 1].IsLdcI4();
                                bool flag6 = flag5;
                                if (flag6) {
                                    MethodSpec methodSpec = methodDef.Body.Instructions[i].Operand as MethodSpec;
                                    uint ldcI4Value = (uint)methodDef.Body.Instructions[i - 1].GetLdcI4Value();
                                    string text = (string)manifestModule.ResolveMethod(methodSpec.MDToken.ToInt32()).Invoke(null, new object[] { ldcI4Value });
                                    methodDef.Body.Instructions[i].OpCode = OpCodes.Nop;
                                    methodDef.Body.Instructions[i - 1].OpCode = OpCodes.Ldstr;
                                    methodDef.Body.Instructions[i - 1].Operand = text;
                                    num++;
                                    bool veryVerbose = Program.veryVerbose;
                                    bool flag7 = veryVerbose;
                                    if (flag7) {
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine(string.Format("Encrypted String Found In Method {0} With Param of {1} the decrypted string is {2}", methodDef.Name, ldcI4Value.ToString(), text));
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return num;
        }
    }
}
