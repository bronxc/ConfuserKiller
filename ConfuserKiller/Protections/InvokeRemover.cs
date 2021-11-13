using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class Invoke_Remover {
      
        public static void run(ModuleDefMD module) {
            int num = 0;
            foreach (TypeDef typeDef in module.GetTypes()) {
                bool flag = !typeDef.IsGlobalModuleType;
                bool flag2 = !flag;
                if (flag2) {
                    foreach (MethodDef methodDef in typeDef.Methods) {
                        bool flag3 = !methodDef.HasBody && !methodDef.Body.HasInstructions;
                        bool flag4 = !flag3;
                        if (flag4) {
                            for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                bool flag5 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call && methodDef.Body.Instructions[i].Operand.ToString().Contains("CallingAssembly");
                                bool flag6 = flag5;
                                if (flag6) {
                                    methodDef.Body.Instructions[i].Operand = (methodDef.Body.Instructions[i].Operand = module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly")));
                                    num++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
