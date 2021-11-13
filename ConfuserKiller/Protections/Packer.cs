using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;

namespace ConfuserKiller.Protections {
    internal class Packer {
    
        public static bool IsPacked(ModuleDefMD module) {
            for (uint num = 1U; num <= module.MetaData.TablesStream.FileTable.Rows; num += 1U) {
                RawFileRow rawFileRow = module.TablesStream.ReadFileRow(num);
                string a = module.StringsStream.ReadNoNull(rawFileRow.Name);
                bool flag = a != "koi";
                if (!flag) {
                    return true;
                }
            }
            return false;
        }

      
        private static void arrayFinder(Local loc) {
            MethodDef entryPoint = Program.module.EntryPoint;
            for (int i = 0; i < entryPoint.Body.Instructions.Count; i++) {
                bool flag = entryPoint.Body.Instructions[i].IsStloc();
                if (flag) {
                    bool flag2 = entryPoint.Body.Instructions[i].GetLocal(entryPoint.Body.Variables) == loc;
                    if (flag2) {
                        bool flag3 = entryPoint.Body.Instructions[i - 1].OpCode == OpCodes.Call && entryPoint.Body.Instructions[i - 2].OpCode == OpCodes.Ldtoken;
                        if (flag3) {
                            FieldDef fieldDef = entryPoint.Body.Instructions[i - 2].Operand as FieldDef;
                            Packer.initialValue = fieldDef.InitialValue;
                            break;
                        }
                    }
                }
            }
        }

     
        public static void findLocal() {
            Module manifestModule = Program.asm.ManifestModule;
            MethodDef entryPoint = Program.module.EntryPoint;
            TypeRef typeRef = Program.module.CorLibTypes.GetTypeRef("System.Runtime.InteropServices", "GCHandle");
            Local[] array = (from i in Program.module.EntryPoint.Body.Variables
                             where i.Type.Namespace == "System.Runtime.InteropServices" && i.Type.TypeName == "GCHandle"
                             select i).ToArray<Local>();
            bool flag = array.Length != 0;
            if (flag) {
                Local local = array[0];
                for (int j = 0; j < entryPoint.Body.Instructions.Count; j++) {
                    bool flag2 = entryPoint.Body.Instructions[j].IsStloc();
                    if (flag2) {
                        bool flag3 = entryPoint.Body.Instructions[j].GetLocal(entryPoint.Body.Variables) == local;
                        if (flag3) {
                            bool flag4 = entryPoint.Body.Instructions[j - 1].OpCode == OpCodes.Call;
                            if (flag4) {
                                bool flag5 = entryPoint.Body.Instructions[j - 2].IsLdcI4();
                                if (flag5) {
                                    bool flag6 = entryPoint.Body.Instructions[j - 3].IsLdloc();
                                    if (flag6) {
                                        MethodDef methodDef = entryPoint.Body.Instructions[j - 1].Operand as MethodDef;
                                        MethodBase methodBase = manifestModule.ResolveMethod(methodDef.MDToken.ToInt32());
                                        object[] array2 = new object[]
                                        {
                                            null,
                                            (uint)entryPoint.Body.Instructions[j - 2].GetLdcI4Value()
                                        };
                                        Local local2 = entryPoint.Body.Instructions[j - 3].GetLocal(entryPoint.Body.Variables);
                                        Packer.arrayFinder(local2);
                                        uint[] array3 = new uint[Packer.initialValue.Length / 4];
                                        Buffer.BlockCopy(Packer.initialValue, 0, array3, 0, Packer.initialValue.Length);
                                        array2[0] = array3;
                                        GCHandle gchandle = (GCHandle)methodBase.Invoke(null, array2);
                                        Program.module = ModuleDefMD.Load((byte[])gchandle.Target);
                                        byte[] array4 = manifestModule.ResolveSignature(285212673);
                                        Packer.epToken = (int)array4[0] | ((int)array4[1] << 8) | ((int)array4[2] << 16) | ((int)array4[3] << 24);
                                        Program.module.EntryPoint = Program.module.ResolveToken(Packer.epToken) as MethodDef;
                                        Program.asm = Assembly.Load((byte[])gchandle.Target);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private static byte[] initialValue;

   
        public static int epToken;
    }
}
