using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class ReferenceProxy {
     
        private static void RemoveJunkMethods(ModuleDefMD module) {
            int num = 0;
            foreach (TypeDef typeDef in module.GetTypes()) {
                List<MethodDef> list = new List<MethodDef>();
                foreach (MethodDef item in typeDef.Methods) {
                    bool flag = ReferenceProxy.junkMethods.Contains(item);
                    bool flag2 = flag;
                    bool flag3 = flag2;
                    if (flag3) {
                        list.Add(item);
                    }
                }
                int num2;
                for (int i = 0; i < list.Count; i = num2 + 1) {
                    typeDef.Methods.Remove(list[i]);
                    num2 = num;
                    num = num2 + 1;
                    num2 = i;
                }
                list.Clear();
            }
            ReferenceProxy.junkMethods.Clear();
            bool flag4 = num > 0;
        }



        public static int ProxyFixer(ModuleDefMD module) {
            int num = 0;
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                            bool flag3 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call;
                            bool flag4 = flag3;
                            if (flag4) {
                                try {
                                    MethodDef methodDef2 = methodDef.Body.Instructions[i].Operand as MethodDef;
                                    bool flag5 = methodDef2 == null;
                                    bool flag6 = !flag5;
                                    bool flag7 = flag6;
                                    if (flag7) {
                                        bool flag8 = !methodDef2.IsStatic || !typeDef.Methods.Contains(methodDef2);
                                        bool flag9 = !flag8;
                                        bool flag10 = flag9;
                                        if (flag10) {
                                            OpCode opCode;
                                            object proxyValues = ReferenceProxy.GetProxyValues(methodDef2, out opCode);
                                            bool flag11 = opCode == null || proxyValues == null;
                                            bool flag12 = !flag11;
                                            bool flag13 = flag12;
                                            if (flag13) {
                                                methodDef.Body.Instructions[i].OpCode = opCode;
                                                methodDef.Body.Instructions[i].Operand = proxyValues;
                                                num++;
                                                bool flag14 = !ReferenceProxy.junkMethods.Contains(methodDef2);
                                                bool flag15 = flag14;
                                                if (flag15) {
                                                    ReferenceProxy.junkMethods.Add(methodDef2);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch {
                                }
                            }
                        }
                    }
                }
            }
            ReferenceProxy.RemoveJunkMethods(module);
            return num;
        }

      
        private static object GetProxyValues(MethodDef method, out OpCode opCode) {
            ReferenceProxy.result = null;
            opCode = null;
            int i = 0;
            while (i < method.Body.Instructions.Count) {
                bool flag = method.Body.Instructions.Count <= 10;
                bool flag2 = flag;
                object obj;
                if (flag2) {
                    bool flag3 = method.Body.Instructions[i].OpCode == OpCodes.Call;
                    bool flag4 = flag3;
                    if (flag4) {
                        opCode = OpCodes.Call;
                        ReferenceProxy.result = method.Body.Instructions[i].Operand;
                        obj = ReferenceProxy.result;
                    }
                    else {
                        bool flag5 = method.Body.Instructions[i].OpCode == OpCodes.Newobj;
                        bool flag6 = flag5;
                        if (flag6) {
                            opCode = OpCodes.Newobj;
                            ReferenceProxy.result = method.Body.Instructions[i].Operand;
                            obj = ReferenceProxy.result;
                        }
                        else {
                            bool flag7 = method.Body.Instructions[i].OpCode == OpCodes.Callvirt;
                            bool flag8 = !flag7;
                            if (flag8) {
                                opCode = null;
                                ReferenceProxy.result = null;
                                i++;
                                continue;
                            }
                            opCode = OpCodes.Callvirt;
                            ReferenceProxy.result = method.Body.Instructions[i].Operand;
                            obj = ReferenceProxy.result;
                        }
                    }
                }
                else {
                    obj = null;
                }
                return obj;
            }
            return ReferenceProxy.result;
        }

 
        private static List<MethodDef> junkMethods = new List<MethodDef>();


        private static object result;
    }
}
