using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class StaticPacker {
     
        public static bool Run(ModuleDefMD module) {
            MethodDef entryPoint = module.EntryPoint;
            uint[] array = StaticPacker.arrayFinder();
            bool flag = array == null;
            bool result;
            if (flag) {
                result = false;
            }
            else {
                uint num = StaticPacker.findLocal();
                bool flag2 = num == 0U;
                if (flag2) {
                    result = false;
                }
                else {
                    byte[] array2 = StaticPacker.Decrypt(StaticPacker.decryptMethod, array, num);
                    bool flag3 = array2 == null;
                    if (flag3) {
                        result = false;
                    }
                    else {
                        int num2 = StaticPacker.epStuff(module.EntryPoint);
                        bool flag4 = num2 == 0;
                        if (flag4) {
                            result = false;
                        }
                        else {
                            byte[] array3 = module.ReadBlob((uint)num2);
                            bool flag5 = array3 == null;
                            if (flag5) {
                                result = false;
                            }
                            else {
                                StaticPacker.epToken = (int)array3[0] | ((int)array3[1] << 8) | ((int)array3[2] << 16) | ((int)array3[3] << 24);
                                Program.module = ModuleDefMD.Load(array2);
                                Program.module.EntryPoint = Program.module.ResolveToken(StaticPacker.epToken) as MethodDef;
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

       
        public static void PopolateArrays(ulong key, out ulong conv) {
            StaticPacker.dst = new uint[16];
            StaticPacker.src = new uint[16];
            ulong num = key;
            for (int i = 0; i < 16; i++) {
                num = num * num % 339722377UL;
                StaticPacker.src[i] = (uint)num;
                StaticPacker.dst[i] = (uint)(num * num % 1145919227UL);
            }
            conv = num;
        }

 
        public static int epStuff(MethodDef method) {
            for (int i = 38; i < method.Body.Instructions.Count; i++) {
                bool flag = method.Body.Instructions[i].IsLdcI4();
                if (flag) {
                    bool flag2 = method.Body.Instructions[i + 1].OpCode == OpCodes.Callvirt && method.Body.Instructions[i + 1].Operand.ToString().Contains("ResolveSignature");
                    if (flag2) {
                        return method.Body.Instructions[i].GetLdcI4Value();
                    }
                }
            }
            return 0;
        }

   
        private static byte[] Decrypt(MethodDef meth, uint[] array, uint num) {
            ulong conv;
            StaticPacker.PopolateArrays((ulong)num, out conv);
            uint[] array2 = StaticPacker.DeriveKey(meth, StaticPacker.dst, StaticPacker.src);
            byte[] data = StaticPacker.decryptDataArray(array);
            byte[] arr = Lzma.Decompress(data);
            return StaticPacker.decryptDecompData(arr, conv);
        }

 
        public static byte[] decryptDecompData(byte[] arr, ulong conv) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] ^= (byte)conv;
                bool flag = (i & 255) == 0;
                if (flag) {
                    conv = conv * conv % 9067703UL;
                }
            }
            return arr;
        }


        public static byte[] decryptDataArray(uint[] DataField) {
            byte[] array = new byte[DataField.Length << 2];
            uint num = 0U;
            for (int i = 0; i < DataField.Length; i++) {
                uint num2 = DataField[i] ^ StaticPacker.dst[i & 15];
                StaticPacker.dst[i & 15] = (StaticPacker.dst[i & 15] ^ num2) + 1037772825U;
                array[(int)num] = (byte)num2;
                array[(int)(num + 1U)] = (byte)(num2 >> 8);
                array[(int)(num + 2U)] = (byte)(num2 >> 16);
                array[(int)(num + 3U)] = (byte)(num2 >> 24);
                num += 4U;
            }
            return array;
        }

    
        private static uint[] DeriveKey(MethodDef DecryptMethod, uint[] dst, uint[] src) {
            Instruction[] array = DecryptMethod.Body.Instructions.ToArray<Instruction>();
            bool flag = array[48].IsLdcI4();
            int num;
            if (flag) {
                num = 48;
            }
            else {
                num = 50;
            }
            int num2 = 0;
            for (int i = num; i < 240; i += 12) {
                uint num3 = (uint)((int)array[i].Operand);
                bool flag2 = array[i - 1].OpCode.Equals(OpCodes.Add);
                if (flag2) {
                    dst[num2] += src[num2];
                }
                bool flag3 = array[i - 1].OpCode.Equals(OpCodes.Mul);
                if (flag3) {
                    dst[num2] *= src[num2];
                }
                bool flag4 = array[i - 1].OpCode.Equals(OpCodes.Xor);
                if (flag4) {
                    dst[num2] ^= src[num2];
                }
                bool flag5 = array[i + 1].OpCode.Equals(OpCodes.Add);
                if (flag5) {
                    dst[num2] += num3;
                }
                bool flag6 = array[i + 1].OpCode.Equals(OpCodes.Mul);
                if (flag6) {
                    dst[num2] *= num3;
                }
                bool flag7 = array[i + 1].OpCode.Equals(OpCodes.Xor);
                if (flag7) {
                    dst[num2] ^= num3;
                }
                num2++;
            }
            return dst;
        }



        public static uint findLocal() {
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
                                        StaticPacker.decryptMethod = entryPoint.Body.Instructions[j - 1].Operand as MethodDef;
                                        return (uint)entryPoint.Body.Instructions[j - 2].GetLdcI4Value();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 0U;
        }

       
        private static uint[] arrayFinder() {
            MethodDef entryPoint = Program.module.EntryPoint;
            for (int i = 0; i < entryPoint.Body.Instructions.Count; i++) {
                bool flag = entryPoint.Body.Instructions[i].OpCode == OpCodes.Stloc_0;
                if (flag) {
                    bool flag2 = entryPoint.Body.Instructions[i - 1].OpCode == OpCodes.Call && entryPoint.Body.Instructions[i - 2].OpCode == OpCodes.Ldtoken;
                    if (flag2) {
                        FieldDef fieldDef = entryPoint.Body.Instructions[i - 2].Operand as FieldDef;
                        byte[] array = fieldDef.InitialValue;
                        uint[] result = new uint[array.Length / 4];
                        Buffer.BlockCopy(array, 0, result, 0, array.Length);
                        return result;
                    }
                }
            }
            return null;
        }

     
        private static byte[] initialValue;

      
        private static uint[] dst;

       
        private static uint[] src;

        private static MethodDef decryptMethod;

        
        public static int epToken;
    }
}
