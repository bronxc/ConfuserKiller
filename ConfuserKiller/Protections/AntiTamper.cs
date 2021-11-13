using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.PE;

namespace ConfuserKiller.Protections {
    internal class AntiTamper {
    
        public static ModuleDefMD UnAntiTamper(ModuleDefMD module, byte[] rawbytes) {
            AntiTamper.dynInstr = new List<Instruction>();
            AntiTamper.initialKeys = new uint[4];
            AntiTamper.cctor = module.GlobalType.FindStaticConstructor();
            AntiTamper.antitamp = AntiTamper.cctor.Body.Instructions[0].Operand as MethodDef;
            bool flag = AntiTamper.antitamp == null;
            bool flag2 = flag;
            ModuleDefMD result;
            if (flag2) {
                result = null;
            }
            else {
                IList<ImageSectionHeader> imageSectionHeaders = module.MetaData.PEImage.ImageSectionHeaders;
                ImageSectionHeader confSec = imageSectionHeaders[0];
                AntiTamper.FindInitialKeys(AntiTamper.antitamp);
                bool flag3 = AntiTamper.initialKeys == null;
                bool flag4 = flag3;
                if (flag4) {
                    result = null;
                }
                else {
                    AntiTamper.input = new MemoryStream(rawbytes);
                    AntiTamper.reader = new BinaryReader(AntiTamper.input);
                    AntiTamper.Hash1(AntiTamper.input, AntiTamper.reader, imageSectionHeaders, confSec);
                    AntiTamper.arrayKeys = AntiTamper.GetArrayKeys();
                    AntiTamper.DecryptMethods(AntiTamper.reader, confSec, AntiTamper.input);
                    ModuleDefMD moduleDefMD = ModuleDefMD.Load(AntiTamper.input);
                    moduleDefMD.GlobalType.FindStaticConstructor().Body.Instructions.RemoveAt(0);
                    result = moduleDefMD;
                }
            }
            return result;
        }


        private static void DecryptMethods(BinaryReader reader, ImageSectionHeader confSec, Stream stream) {
            try {
                int num = (int)(confSec.SizeOfRawData >> 2);
                int pointerToRawData = (int)confSec.PointerToRawData;
                stream.Position = (long)pointerToRawData;
                uint[] array = new uint[num];
                uint num2 = 0U;
                while ((ulong)num2 < (ulong)((long)num)) {
                    uint num3 = reader.ReadUInt32();
                    array[(int)num2] = num3 ^ AntiTamper.arrayKeys[(int)((IntPtr)((long)((ulong)(num2 & 15U))))];
                    AntiTamper.arrayKeys[(int)((IntPtr)((long)((ulong)(num2 & 15U))))] = num3 + 1035675673U;
                    num2 += 1U;
                }
                AntiTamper.byteResult = new byte[num << 2];
                AntiTamper.byteResult = array.SelectMany(new Func<uint, IEnumerable<byte>>(BitConverter.GetBytes)).ToArray<byte>();
                byte[] array2 = AntiTamper.ConvertUInt32ArrayToByteArray(array);
                stream.Position = (long)pointerToRawData;
                stream.Write(AntiTamper.byteResult, 0, AntiTamper.byteResult.Length);
            }
            catch {
                int num4 = (int)(confSec.SizeOfRawData >> 2);
                int pointerToRawData2 = (int)confSec.PointerToRawData;
                stream.Position = (long)pointerToRawData2;
                uint[] array3 = new uint[num4];
                uint num5 = 0U;
                while ((ulong)num5 < (ulong)((long)num4)) {
                    uint num6 = reader.ReadUInt32();
                    array3[(int)num5] = num6 ^ AntiTamper.arrayKeys[(int)((IntPtr)((long)((ulong)(num5 & 15U))))];
                    AntiTamper.arrayKeys[(int)((IntPtr)((long)((ulong)(num5 & 15U))))] = num6 + 1035675793U;
                    num5 += 1U;
                }
                AntiTamper.byteResult = new byte[num4 << 2];
                AntiTamper.byteResult = array3.SelectMany(new Func<uint, IEnumerable<byte>>(BitConverter.GetBytes)).ToArray<byte>();
                byte[] array4 = AntiTamper.ConvertUInt32ArrayToByteArray(array3);
                stream.Position = (long)pointerToRawData2;
                stream.Write(AntiTamper.byteResult, 0, AntiTamper.byteResult.Length);
            }
        }

    
        public static bool? IsTampered(ModuleDefMD module) {
            IList<ImageSectionHeader> imageSectionHeaders = module.MetaData.PEImage.ImageSectionHeaders;
            foreach (ImageSectionHeader imageSectionHeader in imageSectionHeaders) {
                string displayName = imageSectionHeader.DisplayName;
                string text = displayName;
                bool flag = text != null;
                if (flag) {
                    bool flag2 = text == ".text" || text == ".rsrc" || text == ".reloc";
                    if (flag2) {
                        continue;
                    }
                }
                return new bool?(true);
            }
            return null;
        }


        private static byte[] ConvertUInt32ArrayToByteArray(uint[] value) {
            byte[] array = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++) {
                byte[] bytes = BitConverter.GetBytes(value[i]);
                for (int j = 0; j < bytes.Length; j++) {
                    array[i * 4 + j] = bytes[j];
                }
            }
            return array;
        }

        private static void FindInitialKeys(MethodDef antitamp) {
            int count = antitamp.Body.Instructions.Count;
            int num = count - 293;
            for (int i = 0; i < count; i++) {
                Instruction instruction = antitamp.Body.Instructions[i];
                bool flag = instruction.OpCode.Equals(OpCodes.Ldc_I4);
                bool flag2 = flag;
                if (flag2) {
                    bool flag3 = antitamp.Body.Instructions[i + 1].OpCode.Equals(OpCodes.Stloc_S);
                    bool flag4 = flag3;
                    if (flag4) {
                        bool flag5 = antitamp.Body.Instructions[i + 1].Operand.ToString().Contains("V_10");
                        bool flag6 = flag5;
                        if (flag6) {
                            AntiTamper.initialKeys[0] = (uint)((int)instruction.Operand);
                        }
                        bool flag7 = antitamp.Body.Instructions[i + 1].Operand.ToString().Contains("V_11");
                        bool flag8 = flag7;
                        if (flag8) {
                            AntiTamper.initialKeys[1] = (uint)((int)instruction.Operand);
                        }
                        bool flag9 = antitamp.Body.Instructions[i + 1].Operand.ToString().Contains("V_12");
                        bool flag10 = flag9;
                        if (flag10) {
                            AntiTamper.initialKeys[2] = (uint)((int)instruction.Operand);
                        }
                        bool flag11 = antitamp.Body.Instructions[i + 1].Operand.ToString().Contains("V_13");
                        bool flag12 = flag11;
                        if (flag12) {
                            AntiTamper.initialKeys[3] = (uint)((int)instruction.Operand);
                        }
                    }
                }
            }
        }

      
        private static uint[] GetArrayKeys() {
            uint[] array = new uint[16];
            uint[] array2 = new uint[16];
            for (int i = 0; i < 16; i++) {
                array[i] = AntiTamper.initialKeys[3];
                array2[i] = AntiTamper.initialKeys[1];
                AntiTamper.initialKeys[0] = (AntiTamper.initialKeys[1] >> 5) | (AntiTamper.initialKeys[1] << 27);
                AntiTamper.initialKeys[1] = (AntiTamper.initialKeys[2] >> 3) | (AntiTamper.initialKeys[2] << 29);
                AntiTamper.initialKeys[2] = (AntiTamper.initialKeys[3] >> 7) | (AntiTamper.initialKeys[3] << 25);
                AntiTamper.initialKeys[3] = (AntiTamper.initialKeys[0] >> 11) | (AntiTamper.initialKeys[0] << 21);
            }
            return AntiTamper.DeriveKeyAntiTamp(array, array2);
        }


        public static uint[] DeriveKeyAntiTamp(uint[] dst, uint[] src) {
            uint[] array = new uint[16];
            for (int i = 0; i < 16; i++) {
                switch (i % 3) {
                    case 0:
                        array[i] = dst[i] ^ src[i];
                        break;
                    case 1:
                        array[i] = dst[i] * src[i];
                        break;
                    case 2:
                        array[i] = dst[i] + src[i];
                        break;
                }
            }
            return array;
        }

    
        private static void Hash1(Stream stream, BinaryReader reader, IList<ImageSectionHeader> sections, ImageSectionHeader confSec) {
            foreach (ImageSectionHeader imageSectionHeader in sections) {
                bool flag = imageSectionHeader != confSec && imageSectionHeader.DisplayName != "";
                bool flag2 = flag;
                if (flag2) {
                    int num = (int)(imageSectionHeader.SizeOfRawData >> 2);
                    int pointerToRawData = (int)imageSectionHeader.PointerToRawData;
                    stream.Position = (long)pointerToRawData;
                    for (int i = 0; i < num; i++) {
                        uint num2 = reader.ReadUInt32();
                        uint num3 = (AntiTamper.initialKeys[0] ^ num2) + AntiTamper.initialKeys[1] + AntiTamper.initialKeys[2] * AntiTamper.initialKeys[3];
                        AntiTamper.initialKeys[0] = AntiTamper.initialKeys[1];
                        AntiTamper.initialKeys[1] = AntiTamper.initialKeys[2];
                        AntiTamper.initialKeys[1] = AntiTamper.initialKeys[3];
                        AntiTamper.initialKeys[3] = num3;
                    }
                }
            }
        }


        public string DirectoryName = "";


        private static MethodDef antitamp;


        private static uint[] arrayKeys;


        private static byte[] byteResult;


        private static MethodDef cctor;


        private static List<Instruction> dynInstr;


        private static uint[] initialKeys;

        private static BinaryReader reader;


        private static MemoryStream input;
    }
}
