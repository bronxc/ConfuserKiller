using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class StaticStrings {
        
        public static int Run(ModuleDefMD module) {
            MethodDef correct = StaticStrings.firstStep(module);
            uint shit = StaticStrings.getShit(module, correct);
            bool flag = shit == 0U;
            bool flag2 = flag;
            int result;
            if (flag2) {
                result = 0;
            }
            else {
                string arrayName = StaticStrings.getArrayName(module, correct);
                bool flag3 = arrayName == null;
                bool flag4 = flag3;
                if (flag4) {
                    result = 0;
                }
                else {
                    uint[] array = StaticStrings.getArray(module, arrayName);
                    bool flag5 = array == null;
                    bool flag6 = flag5;
                    if (flag6) {
                        result = 0;
                    }
                    else {
                        uint finalShit = StaticStrings.getFinalShit(module, correct);
                        bool flag7 = finalShit == 0U;
                        bool flag8 = flag7;
                        if (flag8) {
                            result = 0;
                        }
                        else {
                            StaticStrings.smethod_1(shit, array, finalShit);
                            StaticStrings.FindString(module);
                            result = StaticStrings.StringsDecrypted;
                        }
                    }
                }
            }
            return result;
        }

        
        public static MethodDef firstStep(ModuleDefMD module) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !methodDef.IsConstructor;
                        bool flag4 = !flag3;
                        if (flag4) {
                            bool flag5 = !methodDef.FullName.ToLower().Contains("module");
                            bool flag6 = !flag5;
                            if (flag6) {
                                for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                    bool flag7 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call;
                                    bool flag8 = flag7;
                                    if (flag8) {
                                        MethodDef methodDef2 = (MethodDef)methodDef.Body.Instructions[i].Operand;
                                        bool flag9 = !methodDef2.HasBody;
                                        bool flag10 = !flag9;
                                        if (flag10) {
                                            bool flag11 = methodDef2.Body.Instructions.Count < 300;
                                            bool flag12 = !flag11;
                                            if (flag12) {
                                                for (int j = 0; j < methodDef2.Body.Instructions.Count; j++) {
                                                    bool flag13 = methodDef2.Body.Instructions[j].OpCode == OpCodes.Stloc_0;
                                                    bool flag14 = flag13;
                                                    if (flag14) {
                                                        bool flag15 = methodDef2.Body.Instructions[j - 1].IsLdcI4();
                                                        bool flag16 = flag15;
                                                        if (flag16) {
                                                            StaticStrings.C.Clear();
                                                            bool flag17 = StaticStrings.dthfs(module, methodDef2);
                                                            bool flag18 = !flag17;
                                                            bool flag19 = !flag18;
                                                            if (flag19) {
                                                                return methodDef2;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


        public static bool dthfs(ModuleDefMD module, MethodDef method) {
            for (int i = 0; i < method.Body.Instructions.Count; i++) {
                bool flag = method.Body.Instructions[i].OpCode == OpCodes.Call;
                bool flag2 = flag;
                if (flag2) {
                    StaticStrings.C.Add(method.Body.Instructions[i]);
                }
            }
            return StaticStrings.SortList();
        }

      
        public static bool SortList() {
            string value = "System.Reflection.Assembly System.Reflection.Assembly::Load(System.Byte[])";
            for (int i = 0; i < StaticStrings.C.Count; i++) {
                bool flag = StaticStrings.C[i].Operand.ToString().Contains(value);
                bool flag2 = flag;
                if (flag2) {
                    return false;
                }
            }
            return true;
        }

       
        public static uint getFinalShit(ModuleDefMD module, MethodDef correct) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !methodDef.IsConstructor;
                        bool flag4 = !flag3;
                        if (flag4) {
                            bool flag5 = !methodDef.FullName.ToLower().Contains("module");
                            bool flag6 = !flag5;
                            if (flag6) {
                                for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                    bool flag7 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call;
                                    bool flag8 = flag7;
                                    if (flag8) {
                                        MethodDef methodDef2 = (MethodDef)methodDef.Body.Instructions[i].Operand;
                                        bool flag9 = methodDef2 != correct;
                                        bool flag10 = !flag9;
                                        if (flag10) {
                                            for (int j = 0; j < methodDef2.Body.Instructions.Count; j++) {
                                                bool flag11 = methodDef2.Body.Instructions[j].OpCode == OpCodes.Stloc_3;
                                                bool flag12 = flag11;
                                                if (flag12) {
                                                    bool flag13 = methodDef2.Body.Instructions[j - 1].IsLdcI4();
                                                    bool flag14 = flag13;
                                                    if (flag14) {
                                                        bool flag15 = methodDef2.Body.Instructions[j - 1].GetLdcI4Value() == 0;
                                                        bool flag16 = !flag15;
                                                        if (flag16) {
                                                            return (uint)methodDef2.Body.Instructions[j - 1].GetLdcI4Value();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 0U;
        }

        public static void FindString(ModuleDefMD module) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                            bool flag3 = i < 1;
                            bool flag4 = !flag3;
                            if (flag4) {
                                bool flag5 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call && methodDef.Body.Instructions[i - 1].IsLdcI4();
                                bool flag6 = flag5;
                                if (flag6) {
                                    try {
                                        StaticStrings.DecryptionMethod = (MethodSpec)methodDef.Body.Instructions[i].Operand;
                                        bool flag7 = StaticStrings.DecryptionMethod.FullName.ToLower().Contains("string");
                                        bool flag8 = flag7;
                                        if (flag8) {
                                            int ldcI4Value = methodDef.Body.Instructions[i - 1].GetLdcI4Value();
                                            string paramValues = StaticStrings.GetParamValues(module, StaticStrings.DecryptionMethod, (uint)ldcI4Value);
                                            bool flag9 = paramValues != null;
                                            bool flag10 = flag9;
                                            if (flag10) {
                                                methodDef.Body.Instructions[i].OpCode = OpCodes.Nop;
                                                methodDef.Body.Instructions[i - 1].OpCode = OpCodes.Ldstr;
                                                methodDef.Body.Instructions[i - 1].Operand = paramValues;
                                                bool veryVerbose = Program.veryVerbose;
                                                bool flag11 = veryVerbose;
                                                if (flag11) {
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.WriteLine(string.Format("Encrypted String Found In Method {0} With Param of {1} the decrypted string is {2}", methodDef.Name, ldcI4Value.ToString(), paramValues));
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                }
                                                StaticStrings.StringsDecrypted++;
                                            }
                                        }
                                    }
                                    catch (Exception ex) {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

 
        public static string GetParamValues(ModuleDefMD module, MethodSpec decryption, uint param) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !methodDef.FullName.Contains(decryption.Name);
                        bool flag4 = !flag3;
                        if (flag4) {
                            for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                bool flag5 = methodDef.Body.Instructions[i].OpCode == OpCodes.Mul;
                                bool flag6 = flag5;
                                if (flag6) {
                                    bool flag7 = methodDef.Body.Instructions[i - 1].IsLdcI4() && methodDef.Body.Instructions[i + 1].IsLdcI4();
                                    bool flag8 = flag7;
                                    if (flag8) {
                                        return StaticStrings.smethod_6<string>(param, (uint)methodDef.Body.Instructions[i - 1].GetLdcI4Value(), (uint)methodDef.Body.Instructions[i + 1].GetLdcI4Value());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

      
        internal static T smethod_6<T>(uint uint_0, uint param1, uint param2) {
            uint_0 = (uint_0 * param1) ^ param2;
            uint num = uint_0 >> 30;
            T result = default(T);
            uint_0 &= 1073741823U;
            uint_0 <<= 2;
            num = 3U;
            bool flag = (ulong)num == 3UL;
            bool flag2 = flag;
            if (flag2) {
                int count = (int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 8) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 16) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 24);
                result = (T)((object)string.Intern(Encoding.UTF8.GetString(StaticStrings.byte_0, (int)uint_0, count)));
            }
            else {
                bool flag3 = (ulong)num == 2UL;
                bool flag4 = flag3;
                if (flag4) {
                    T[] array = new T[1];
                   
                    result = array[0];
                }
                else {
                    bool flag5 = (ulong)num == 0UL;
                    bool flag6 = flag5;
                    if (flag6) {
                        int num2 = (int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 8) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 16) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 24);
                        int length = (int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 8) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 16) | ((int)StaticStrings.byte_0[(int)(uint)((UIntPtr)(uint_0++))] << 24);
                        Array array2 = Array.CreateInstance(typeof(T).GetElementType(), length);
                        Buffer.BlockCopy(StaticStrings.byte_0, (int)uint_0, array2, 0, num2 - 4);
                        result = (T)((object)array2);
                    }
                }
            }
            return result;
        }

       
        public static uint getShit(ModuleDefMD module, MethodDef correct) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !methodDef.IsConstructor;
                        bool flag4 = !flag3;
                        if (flag4) {
                            bool flag5 = !methodDef.FullName.ToLower().Contains("module");
                            bool flag6 = !flag5;
                            if (flag6) {
                                for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                    bool flag7 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call;
                                    bool flag8 = flag7;
                                    if (flag8) {
                                        MethodDef methodDef2 = (MethodDef)methodDef.Body.Instructions[i].Operand;
                                        bool flag9 = methodDef2 != correct;
                                        bool flag10 = !flag9;
                                        if (flag10) {
                                            for (int j = 0; j < methodDef2.Body.Instructions.Count; j++) {
                                                bool flag11 = methodDef2.Body.Instructions[j].OpCode == OpCodes.Stloc_0;
                                                bool flag12 = flag11;
                                                if (flag12) {
                                                    bool flag13 = methodDef2.Body.Instructions[j - 1].IsLdcI4();
                                                    bool flag14 = flag13;
                                                    if (flag14) {
                                                        bool flag15 = methodDef2.Body.Instructions[j - 1].GetLdcI4Value() == 0;
                                                        bool flag16 = !flag15;
                                                        if (flag16) {
                                                            return (uint)methodDef2.Body.Instructions[j - 1].GetLdcI4Value();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 0U;
        }

     

        public static string getArrayName(ModuleDefMD module, MethodDef correct) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !methodDef.IsConstructor;
                        bool flag4 = !flag3;
                        if (flag4) {
                            bool flag5 = !methodDef.FullName.ToLower().Contains("module");
                            bool flag6 = !flag5;
                            if (flag6) {
                                for (int i = 0; i < methodDef.Body.Instructions.Count; i++) {
                                    bool flag7 = methodDef.Body.Instructions[i].OpCode == OpCodes.Call;
                                    bool flag8 = flag7;
                                    if (flag8) {
                                        MethodDef methodDef2 = (MethodDef)methodDef.Body.Instructions[i].Operand;
                                        bool flag9 = methodDef2 != correct;
                                        bool flag10 = !flag9;
                                        if (flag10) {
                                            for (int j = 0; j < methodDef2.Body.Instructions.Count; j++) {
                                                bool flag11 = methodDef2.Body.Instructions[j].OpCode == OpCodes.Stloc_1;
                                                bool flag12 = flag11;
                                                if (flag12) {
                                                    bool flag13 = methodDef2.Body.Instructions[j - 2].OpCode == OpCodes.Ldtoken;
                                                    bool flag14 = flag13;
                                                    if (flag14) {
                                                        return methodDef2.Body.Instructions[j - 2].Operand.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

      
        public static uint[] getArray(ModuleDefMD module, string fieldName) {
            foreach (TypeDef typeDef in module.Types) {
                foreach (FieldDef fieldDef in typeDef.Fields) {
                    bool flag = !fieldName.ToLower().Contains(fieldDef.Name.ToLower());
                    bool flag2 = !flag;
                    if (flag2) {
                        bool flag3 = !fieldDef.HasFieldRVA;
                        bool flag4 = !flag3;
                        if (flag4) {
                            bool flag5 = fieldDef.InitialValue.Length == 0;
                            bool flag6 = !flag5;
                            if (flag6) {
                                bool flag7 = !fieldDef.FullName.ToLower().Contains("module");
                                bool flag8 = !flag7;
                                if (flag8) {
                                    bool flag9 = !fieldDef.IsStatic;
                                    bool flag10 = !flag9;
                                    if (flag10) {
                                        bool flag11 = !fieldDef.IsAssembly;
                                        bool flag12 = !flag11;
                                        if (flag12) {
                                            byte[] initialValue = fieldDef.InitialValue;
                                            uint[] array = new uint[initialValue.Length / 4];
                                            Buffer.BlockCopy(initialValue, 0, array, 0, initialValue.Length);
                                            return array;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

   
        internal static void smethod_1(uint numis, uint[] arrayy, uint val3) {
            uint[] array = new uint[16];
            uint num = val3;
            for (int i = 0; i < 16; i++) {
                num ^= num >> 12;
                num ^= num << 25;
                num ^= num >> 27;
                array[i] = num;
            }
            int num2 = 0;
            int num3 = 0;
            uint[] array2 = new uint[16];
            byte[] array3 = new byte[numis * 4U];
            while ((long)num2 < (long)((ulong)numis)) {
                for (int j = 0; j < 16; j++) {
                    array2[j] = arrayy[num2 + j];
                }
                array2[0] = array2[0] ^ array[0];
                array2[1] = array2[1] ^ array[1];
                array2[2] = array2[2] ^ array[2];
                array2[3] = array2[3] ^ array[3];
                array2[4] = array2[4] ^ array[4];
                array2[5] = array2[5] ^ array[5];
                array2[6] = array2[6] ^ array[6];
                array2[7] = array2[7] ^ array[7];
                array2[8] = array2[8] ^ array[8];
                array2[9] = array2[9] ^ array[9];
                array2[10] = array2[10] ^ array[10];
                array2[11] = array2[11] ^ array[11];
                array2[12] = array2[12] ^ array[12];
                array2[13] = array2[13] ^ array[13];
                array2[14] = array2[14] ^ array[14];
                array2[15] = array2[15] ^ array[15];
                for (int k = 0; k < 16; k++) {
                    uint num4 = array2[k];
                    array3[num3++] = (byte)num4;
                    array3[num3++] = (byte)(num4 >> 8);
                    array3[num3++] = (byte)(num4 >> 16);
                    array3[num3++] = (byte)(num4 >> 24);
                    array[k] ^= num4;
                }
                num2 += 16;
            }
            StaticStrings.byte_0 = StaticStrings.smethod_0(array3);
        }

       
        internal static byte[] smethod_0(byte[] byte_1) {
            MemoryStream memoryStream = new MemoryStream(byte_1);
            Class1 @class = new Class1();
            byte[] buffer = new byte[5];
            memoryStream.Read(buffer, 0, 5);
            @class.method_5(buffer);
            long num = 0L;
            for (int i = 0; i < 8; i++) {
                int num2 = memoryStream.ReadByte();
                num |= (long)((long)((ulong)((byte)num2)) << 8 * i);
            }
            byte[] array = new byte[(int)num];
            MemoryStream stream_ = new MemoryStream(array, true);
            long long_ = memoryStream.Length - 13L;
            @class.method_4(memoryStream, stream_, long_, num);
            return array;
        }

  
        public static List<Instruction> C = new List<Instruction>();

   
        public static int StringsDecrypted = 0;

       
        private static byte[] byte_0;

        private static MethodSpec DecryptionMethod;
    }
}
