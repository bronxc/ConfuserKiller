using System;
using System.IO;

namespace ConfuserKiller.Protections {

    internal static class Lzma {
    
        public static byte[] Decompress(byte[] data) {
            MemoryStream memoryStream = new MemoryStream(data);
            Lzma.LzmaDecoder lzmaDecoder = new Lzma.LzmaDecoder();
            byte[] array = new byte[5];
            memoryStream.Read(array, 0, 5);
            lzmaDecoder.SetDecoderProperties(array);
            long num = 0L;
            for (int i = 0; i < 8; i++) {
                int num2 = memoryStream.ReadByte();
                num |= (long)((long)((ulong)((byte)num2)) << 8 * i);
            }
            byte[] array2 = new byte[num];
            MemoryStream outStream = new MemoryStream(array2, true);
            long inSize = memoryStream.Length - 13L;
            lzmaDecoder.Code(memoryStream, outStream, inSize, num);
            return array2;
        }

    
        private const uint kAlignTableSize = 16U;

   
        private const uint kEndPosModelIndex = 14U;


        private const uint kMatchMinLen = 2U;

    
        private const int kNumAlignBits = 4;

        private const uint kNumFullDistances = 128U;

     
        private const int kNumHighLenBits = 8;

        private const uint kNumLenToPosStates = 4U;

    
        private const int kNumLowLenBits = 3;


        private const uint kNumLowLenSymbols = 8U;

     
        private const int kNumMidLenBits = 3;


        private const uint kNumMidLenSymbols = 8U;

      
        private const int kNumPosSlotBits = 6;

        private const int kNumPosStatesBitsMax = 4;

       
        private const uint kNumPosStatesMax = 16U;


        private const uint kNumStates = 12U;

     
        private const uint kStartPosModelIndex = 4U;

     
        private struct BitDecoder {
           
            public void Init() {
                this.Prob = 1024U;
            }

        
            public uint Decode(Lzma.Decoder rangeDecoder) {
                uint num = (rangeDecoder.Range >> 11) * this.Prob;
                bool flag = rangeDecoder.Code < num;
                bool flag2 = flag;
                uint result;
                if (flag2) {
                    rangeDecoder.Range = num;
                    this.Prob += 2048U - this.Prob >> 5;
                    bool flag3 = rangeDecoder.Range < 16777216U;
                    bool flag4 = flag3;
                    if (flag4) {
                        rangeDecoder.Code = (rangeDecoder.Code << 8) | (uint)((byte)rangeDecoder.Stream.ReadByte());
                        rangeDecoder.Range <<= 8;
                    }
                    result = 0U;
                }
                else {
                    rangeDecoder.Range -= num;
                    rangeDecoder.Code -= num;
                    this.Prob -= this.Prob >> 5;
                    bool flag5 = rangeDecoder.Range < 16777216U;
                    bool flag6 = flag5;
                    if (flag6) {
                        rangeDecoder.Code = (rangeDecoder.Code << 8) | (uint)((byte)rangeDecoder.Stream.ReadByte());
                        rangeDecoder.Range <<= 8;
                    }
                    result = 1U;
                }
                return result;
            }

          
            public const int kNumBitModelTotalBits = 11;


            public const uint kBitModelTotal = 2048U;

         
            private const int kNumMoveBits = 5;

  
            private uint Prob;
        }

      
        private struct BitTreeDecoder {
     
            public BitTreeDecoder(int numBitLevels) {
                this.NumBitLevels = numBitLevels;
                this.Models = new Lzma.BitDecoder[1 << numBitLevels];
            }

            
            public void Init() {
                uint num = 1U;
                while ((ulong)num < 1UL << (this.NumBitLevels & 31)) {
                    this.Models[(int)num].Init();
                    num += 1U;
                }
            }

   
            public uint Decode(Lzma.Decoder rangeDecoder) {
                uint num = 1U;
                for (int i = this.NumBitLevels; i > 0; i--) {
                    num = (num << 1) + this.Models[(int)num].Decode(rangeDecoder);
                }
                return num - (1U << this.NumBitLevels);
            }

        
            public uint ReverseDecode(Lzma.Decoder rangeDecoder) {
                uint num = 1U;
                uint num2 = 0U;
                for (int i = 0; i < this.NumBitLevels; i++) {
                    uint num3 = this.Models[(int)num].Decode(rangeDecoder);
                    num <<= 1;
                    num += num3;
                    num2 |= num3 << i;
                }
                return num2;
            }

         
            public static uint ReverseDecode(Lzma.BitDecoder[] Models, uint startIndex, Lzma.Decoder rangeDecoder, int NumBitLevels) {
                uint num = 1U;
                uint num2 = 0U;
                for (int i = 0; i < NumBitLevels; i++) {
                    uint num3 = Models[(int)(startIndex + num)].Decode(rangeDecoder);
                    num <<= 1;
                    num += num3;
                    num2 |= num3 << i;
                }
                return num2;
            }


            private readonly Lzma.BitDecoder[] Models;

          
            private readonly int NumBitLevels;
        }

  
        private class Decoder {
       
            public uint DecodeDirectBits(int numTotalBits) {
                uint num = this.Range;
                uint num2 = this.Code;
                uint num3 = 0U;
                for (int i = numTotalBits; i > 0; i--) {
                    num >>= 1;
                    uint num4 = num2 - num >> 31;
                    num2 -= num & (num4 - 1U);
                    num3 = (num3 << 1) | (1U - num4);
                    bool flag = num < 16777216U;
                    bool flag2 = flag;
                    if (flag2) {
                        num2 = (num2 << 8) | (uint)((byte)this.Stream.ReadByte());
                        num <<= 8;
                    }
                }
                this.Range = num;
                this.Code = num2;
                return num3;
            }

          
            public void Init(Stream stream) {
                this.Stream = stream;
                this.Code = 0U;
                this.Range = uint.MaxValue;
                for (int i = 0; i < 5; i++) {
                    this.Code = (this.Code << 8) | (uint)((byte)this.Stream.ReadByte());
                }
            }

            
            public void Normalize() {
                while (this.Range < 16777216U) {
                    this.Code = (this.Code << 8) | (uint)((byte)this.Stream.ReadByte());
                    this.Range <<= 8;
                }
            }

            
            public void ReleaseStream() {
                this.Stream = null;
            }

            
            public uint Code;

     
            public const uint kTopValue = 16777216U;

          
            public uint Range;


            public Stream Stream;
        }

       
        private class LzmaDecoder {
         
            public LzmaDecoder() {
                int num = 0;
                while ((long)num < 4L) {
                    this.m_PosSlotDecoder[num] = new Lzma.BitTreeDecoder(6);
                    num++;
                }
            }

            
            public void Code(Stream inStream, Stream outStream, long inSize, long outSize) {
                this.Init(inStream, outStream);
                Lzma.State state = default(Lzma.State);
                state.Init();
                uint num = 0U;
                uint num2 = 0U;
                uint num3 = 0U;
                uint num4 = 0U;
                ulong num5 = 0UL;
                bool flag = num5 < (ulong)outSize;
                bool flag2 = flag;
                if (flag2) {
                    this.m_IsMatchDecoders[(int)((int)state.Index << 4)].Decode(this.m_RangeDecoder);
                    state.UpdateChar();
                    byte b = this.m_LiteralDecoder.DecodeNormal(this.m_RangeDecoder, 0U, 0);
                    this.m_OutWindow.PutByte(b);
                    num5 += 1UL;
                }
                while (num5 < (ulong)outSize) {
                    uint num6 = (uint)num5 & this.m_PosStateMask;
                    bool flag3 = this.m_IsMatchDecoders[(int)((state.Index << 4) + num6)].Decode(this.m_RangeDecoder) == 0U;
                    bool flag4 = flag3;
                    if (flag4) {
                        byte @byte = this.m_OutWindow.GetByte(0U);
                        bool flag5 = !state.IsCharState();
                        bool flag6 = flag5;
                        byte b2;
                        if (flag6) {
                            b2 = this.m_LiteralDecoder.DecodeWithMatchByte(this.m_RangeDecoder, (uint)num5, @byte, this.m_OutWindow.GetByte(num));
                        }
                        else {
                            b2 = this.m_LiteralDecoder.DecodeNormal(this.m_RangeDecoder, (uint)num5, @byte);
                        }
                        this.m_OutWindow.PutByte(b2);
                        state.UpdateChar();
                        num5 += 1UL;
                    }
                    else {
                        bool flag7 = this.m_IsRepDecoders[(int)state.Index].Decode(this.m_RangeDecoder) == 1U;
                        bool flag8 = flag7;
                        uint num8;
                        if (flag8) {
                            bool flag9 = this.m_IsRepG0Decoders[(int)state.Index].Decode(this.m_RangeDecoder) == 0U;
                            bool flag10 = flag9;
                            if (flag10) {
                                bool flag11 = this.m_IsRep0LongDecoders[(int)((state.Index << 4) + num6)].Decode(this.m_RangeDecoder) == 0U;
                                bool flag12 = flag11;
                                if (flag12) {
                                    state.UpdateShortRep();
                                    this.m_OutWindow.PutByte(this.m_OutWindow.GetByte(num));
                                    num5 += 1UL;
                                    continue;
                                }
                            }
                            else {
                                bool flag13 = this.m_IsRepG1Decoders[(int)state.Index].Decode(this.m_RangeDecoder) == 0U;
                                bool flag14 = flag13;
                                uint num7;
                                if (flag14) {
                                    num7 = num2;
                                }
                                else {
                                    bool flag15 = this.m_IsRepG2Decoders[(int)state.Index].Decode(this.m_RangeDecoder) == 0U;
                                    bool flag16 = flag15;
                                    if (flag16) {
                                        num7 = num3;
                                    }
                                    else {
                                        num7 = num4;
                                        num4 = num3;
                                    }
                                    num3 = num2;
                                }
                                num2 = num;
                                num = num7;
                            }
                            num8 = this.m_RepLenDecoder.Decode(this.m_RangeDecoder, num6) + 2U;
                            state.UpdateRep();
                        }
                        else {
                            num4 = num3;
                            num3 = num2;
                            num2 = num;
                            num8 = 2U + this.m_LenDecoder.Decode(this.m_RangeDecoder, num6);
                            state.UpdateMatch();
                            uint num9 = this.m_PosSlotDecoder[(int)Lzma.LzmaDecoder.GetLenToPosState(num8)].Decode(this.m_RangeDecoder);
                            bool flag17 = num9 >= 4U;
                            bool flag18 = flag17;
                            if (flag18) {
                                int num10 = (int)((num9 >> 1) - 1U);
                                num = (2U | (num9 & 1U)) << num10;
                                bool flag19 = num9 < 14U;
                                bool flag20 = flag19;
                                if (flag20) {
                                    num += Lzma.BitTreeDecoder.ReverseDecode(this.m_PosDecoders, num - num9 - 1U, this.m_RangeDecoder, num10);
                                }
                                else {
                                    num += this.m_RangeDecoder.DecodeDirectBits(num10 - 4) << 4;
                                    num += this.m_PosAlignDecoder.ReverseDecode(this.m_RangeDecoder);
                                }
                            }
                            else {
                                num = num9;
                            }
                        }
                        bool flag21 = ((ulong)num >= num5 || num >= this.m_DictionarySizeCheck) && num == uint.MaxValue;
                        bool flag22 = flag21;
                        if (flag22) {
                            break;
                        }
                        this.m_OutWindow.CopyBlock(num, num8);
                        num5 += (ulong)num8;
                    }
                }
                this.m_OutWindow.Flush();
                this.m_OutWindow.ReleaseStream();
                this.m_RangeDecoder.ReleaseStream();
            }

       
            private static uint GetLenToPosState(uint len) {
                len -= 2U;
                bool flag = len < 4U;
                bool flag2 = flag;
                uint result;
                if (flag2) {
                    result = len;
                }
                else {
                    result = 3U;
                }
                return result;
            }

       
            private void Init(Stream inStream, Stream outStream) {
                this.m_RangeDecoder.Init(inStream);
                this.m_OutWindow.Init(outStream, this._solid);
                for (uint num = 0U; num < 12U; num += 1U) {
                    for (uint num2 = 0U; num2 <= this.m_PosStateMask; num2 += 1U) {
                        uint num3 = (num << 4) + num2;
                        this.m_IsMatchDecoders[(int)num3].Init();
                        this.m_IsRep0LongDecoders[(int)num3].Init();
                    }
                    this.m_IsRepDecoders[(int)num].Init();
                    this.m_IsRepG0Decoders[(int)num].Init();
                    this.m_IsRepG1Decoders[(int)num].Init();
                    this.m_IsRepG2Decoders[(int)num].Init();
                }
                this.m_LiteralDecoder.Init();
                for (uint num4 = 0U; num4 < 4U; num4 += 1U) {
                    this.m_PosSlotDecoder[(int)num4].Init();
                }
                for (uint num5 = 0U; num5 < 114U; num5 += 1U) {
                    this.m_PosDecoders[(int)num5].Init();
                }
                this.m_LenDecoder.Init();
                this.m_RepLenDecoder.Init();
                this.m_PosAlignDecoder.Init();
            }


            public void SetDecoderProperties(byte[] properties) {
                int lc = (int)(properties[0] % 9);
                int num = (int)(properties[0] / 9);
                int lp = num % 5;
                int posBitsProperties = num / 5;
                uint num2 = 0U;
                for (int i = 0; i < 4; i++) {
                    num2 += (uint)((uint)properties[1 + i] << i * 8);
                }
                this.SetDictionarySize(num2);
                this.SetLiteralProperties(lp, lc);
                this.SetPosBitsProperties(posBitsProperties);
            }

            private void SetDictionarySize(uint dictionarySize) {
                bool flag = this.m_DictionarySize != dictionarySize;
                bool flag2 = flag;
                if (flag2) {
                    this.m_DictionarySize = dictionarySize;
                    this.m_DictionarySizeCheck = Math.Max(this.m_DictionarySize, 1U);
                    uint windowSize = Math.Max(this.m_DictionarySizeCheck, 4096U);
                    this.m_OutWindow.Create(windowSize);
                }
            }

 
            private void SetLiteralProperties(int lp, int lc) {
                this.m_LiteralDecoder.Create(lp, lc);
            }

   
            private void SetPosBitsProperties(int pb) {
                uint num = 1U << pb;
                this.m_LenDecoder.Create(num);
                this.m_RepLenDecoder.Create(num);
                this.m_PosStateMask = num - 1U;
            }

         
            private bool _solid = false;

            
            private uint m_DictionarySize = uint.MaxValue;

            
            private uint m_DictionarySizeCheck;

           
            private readonly Lzma.BitDecoder[] m_IsMatchDecoders = new Lzma.BitDecoder[192];

 
            private readonly Lzma.BitDecoder[] m_IsRep0LongDecoders = new Lzma.BitDecoder[192];

            private readonly Lzma.BitDecoder[] m_IsRepDecoders = new Lzma.BitDecoder[12];

           
            private readonly Lzma.BitDecoder[] m_IsRepG0Decoders = new Lzma.BitDecoder[12];


            private readonly Lzma.BitDecoder[] m_IsRepG1Decoders = new Lzma.BitDecoder[12];

         
            private readonly Lzma.BitDecoder[] m_IsRepG2Decoders = new Lzma.BitDecoder[12];


            private readonly Lzma.LzmaDecoder.LenDecoder m_LenDecoder = new Lzma.LzmaDecoder.LenDecoder();

            
            private readonly Lzma.LzmaDecoder.LiteralDecoder m_LiteralDecoder = new Lzma.LzmaDecoder.LiteralDecoder();


            private readonly Lzma.OutWindow m_OutWindow = new Lzma.OutWindow();

         
            private Lzma.BitTreeDecoder m_PosAlignDecoder = new Lzma.BitTreeDecoder(4);

      
            private readonly Lzma.BitDecoder[] m_PosDecoders = new Lzma.BitDecoder[114];

         
            private readonly Lzma.BitTreeDecoder[] m_PosSlotDecoder = new Lzma.BitTreeDecoder[4];


            private uint m_PosStateMask;

            private readonly Lzma.Decoder m_RangeDecoder = new Lzma.Decoder();

          
            private readonly Lzma.LzmaDecoder.LenDecoder m_RepLenDecoder = new Lzma.LzmaDecoder.LenDecoder();

         
            private class LenDecoder {
             


                public void Create(uint numPosStates) {
                    for (uint num = this.m_NumPosStates; num < numPosStates; num += 1U) {
                        this.m_LowCoder[(int)num] = new Lzma.BitTreeDecoder(3);
                        this.m_MidCoder[(int)num] = new Lzma.BitTreeDecoder(3);
                    }
                    this.m_NumPosStates = numPosStates;
                }

            
                public uint Decode(Lzma.Decoder rangeDecoder, uint posState) {
                    bool flag = this.m_Choice.Decode(rangeDecoder) == 0U;
                    bool flag2 = flag;
                    uint result;
                    if (flag2) {
                        result = this.m_LowCoder[(int)posState].Decode(rangeDecoder);
                    }
                    else {
                        uint num = 8U;
                        bool flag3 = this.m_Choice2.Decode(rangeDecoder) == 0U;
                        bool flag4 = flag3;
                        if (flag4) {
                            num += this.m_MidCoder[(int)posState].Decode(rangeDecoder);
                        }
                        else {
                            num += 8U;
                            num += this.m_HighCoder.Decode(rangeDecoder);
                        }
                        result = num;
                    }
                    return result;
                }

          
                public void Init() {
                    this.m_Choice.Init();
                    for (uint num = 0U; num < this.m_NumPosStates; num += 1U) {
                        this.m_LowCoder[(int)num].Init();
                        this.m_MidCoder[(int)num].Init();
                    }
                    this.m_Choice2.Init();
                    this.m_HighCoder.Init();
                }

             
                private Lzma.BitDecoder m_Choice = default(Lzma.BitDecoder);

              
                private Lzma.BitDecoder m_Choice2 = default(Lzma.BitDecoder);

              
                private Lzma.BitTreeDecoder m_HighCoder = new Lzma.BitTreeDecoder(8);

               
                private readonly Lzma.BitTreeDecoder[] m_LowCoder = new Lzma.BitTreeDecoder[16];

                 private readonly Lzma.BitTreeDecoder[] m_MidCoder = new Lzma.BitTreeDecoder[16];

           
                private uint m_NumPosStates;
            }


            private class LiteralDecoder {
               
                public void Create(int numPosBits, int numPrevBits) {
                    bool flag = this.m_Coders == null || this.m_NumPrevBits != numPrevBits || this.m_NumPosBits != numPosBits;
                    bool flag2 = flag;
                    if (flag2) {
                        this.m_NumPosBits = numPosBits;
                        this.m_PosMask = (1U << numPosBits) - 1U;
                        this.m_NumPrevBits = numPrevBits;
                        uint num = 1U << this.m_NumPrevBits + this.m_NumPosBits;
                        this.m_Coders = new Lzma.LzmaDecoder.LiteralDecoder.Decoder2[num];
                        for (uint num2 = 0U; num2 < num; num2 += 1U) {
                            this.m_Coders[(int)num2].Create();
                        }
                    }
                }

    
                public byte DecodeNormal(Lzma.Decoder rangeDecoder, uint pos, byte prevByte) {
                    return this.m_Coders[(int)this.GetState(pos, prevByte)].DecodeNormal(rangeDecoder);
                }

                public byte DecodeWithMatchByte(Lzma.Decoder rangeDecoder, uint pos, byte prevByte, byte matchByte) {
                    return this.m_Coders[(int)this.GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte);
                }

              
                private uint GetState(uint pos, byte prevByte) {
                    return ((pos & this.m_PosMask) << this.m_NumPrevBits) + (uint)(prevByte >> 8 - this.m_NumPrevBits);
                }

           
                public void Init() {
                    uint num = 1U << this.m_NumPrevBits + this.m_NumPosBits;
                    for (uint num2 = 0U; num2 < num; num2 += 1U) {
                        this.m_Coders[(int)num2].Init();
                    }
                }

      
                private Lzma.LzmaDecoder.LiteralDecoder.Decoder2[] m_Coders;

                private int m_NumPosBits;

      
                private int m_NumPrevBits;

             
                private uint m_PosMask;

             
                private struct Decoder2 {
    
                    public void Create() {
                        this.m_Decoders = new Lzma.BitDecoder[768];
                    }

            
                    public void Init() {
                        for (int i = 0; i < 768; i++) {
                            this.m_Decoders[i].Init();
                        }
                    }

                    public byte DecodeNormal(Lzma.Decoder rangeDecoder) {
                        uint num = 1U;
                        do {
                            num = (num << 1) | this.m_Decoders[(int)num].Decode(rangeDecoder);
                        }
                        while (num < 256U);
                        return (byte)num;
                    }

                    
                    public byte DecodeWithMatchByte(Lzma.Decoder rangeDecoder, byte matchByte) {
                        uint num = 1U;
                        for (; ; )
                        {
                            uint num2 = (uint)((matchByte >> 7) & 1);
                            matchByte = (byte)(matchByte << 1);
                            uint num3 = this.m_Decoders[(int)((IntPtr)((long)((ulong)((1U + num2 << 8) + num))))].Decode(rangeDecoder);
                            num = (num << 1) | num3;
                            bool flag = num2 != num3;
                            bool flag2 = flag;
                            if (flag2) {
                                break;
                            }
                            bool flag3 = num >= 256U;
                            if (flag3) {
                                goto Block_2;
                            }
                        }
                        while (num < 256U) {
                            num = (num << 1) | this.m_Decoders[(int)num].Decode(rangeDecoder);
                        }
                        Block_2:
                        return (byte)num;
                    }

                 
                    private Lzma.BitDecoder[] m_Decoders;
                }
            }
        }

      
        private class OutWindow {
            
            public void CopyBlock(uint distance, uint len) {
                uint num = this._pos - distance - 1U;
                bool flag = num >= this._windowSize;
                bool flag2 = flag;
                if (flag2) {
                    num += this._windowSize;
                }
                while (len > 0U) {
                    bool flag3 = num >= this._windowSize;
                    bool flag4 = flag3;
                    if (flag4) {
                        num = 0U;
                    }
                    byte[] buffer = this._buffer;
                    uint pos = this._pos;
                    this._pos = pos + 1U;
                    buffer[(int)pos] = this._buffer[(int)num++];
                    bool flag5 = this._pos >= this._windowSize;
                    bool flag6 = flag5;
                    if (flag6) {
                        this.Flush();
                    }
                    len -= 1U;
                }
            }

          
            public void Create(uint windowSize) {
                bool flag = this._windowSize != windowSize;
                bool flag2 = flag;
                if (flag2) {
                    this._buffer = new byte[windowSize];
                }
                this._windowSize = windowSize;
                this._pos = 0U;
                this._streamPos = 0U;
            }

       
            public void Flush() {
                uint num = this._pos - this._streamPos;
                bool flag = num > 0U;
                bool flag2 = flag;
                if (flag2) {
                    this._stream.Write(this._buffer, (int)this._streamPos, (int)num);
                    bool flag3 = this._pos >= this._windowSize;
                    bool flag4 = flag3;
                    if (flag4) {
                        this._pos = 0U;
                    }
                    this._streamPos = this._pos;
                }
            }
    
            public byte GetByte(uint distance) {
                uint num = this._pos - distance - 1U;
                bool flag = num >= this._windowSize;
                bool flag2 = flag;
                if (flag2) {
                    num += this._windowSize;
                }
                return this._buffer[(int)num];
            }

          
            public void Init(Stream stream, bool solid) {
                this.ReleaseStream();
                this._stream = stream;
                bool flag = !solid;
                bool flag2 = flag;
                if (flag2) {
                    this._streamPos = 0U;
                    this._pos = 0U;
                }
            }

          
            public void PutByte(byte b) {
                byte[] buffer = this._buffer;
                uint pos = this._pos;
                this._pos = pos + 1U;
                buffer[(int)pos] = b;
                bool flag = this._pos >= this._windowSize;
                bool flag2 = flag;
                if (flag2) {
                    this.Flush();
                }
            }

          
            public void ReleaseStream() {
                this.Flush();
                this._stream = null;
                Buffer.BlockCopy(new byte[this._buffer.Length], 0, this._buffer, 0, this._buffer.Length);
            }

            private byte[] _buffer;

       
            private uint _pos;


            private Stream _stream;

        
            private uint _streamPos;

    
            private uint _windowSize;
        }

 
        private struct State {
          
            public void Init() {
                this.Index = 0U;
            }

         
            public void UpdateChar() {
                bool flag = this.Index < 4U;
                bool flag2 = flag;
                if (flag2) {
                    this.Index = 0U;
                }
                else {
                    bool flag3 = this.Index < 10U;
                    bool flag4 = flag3;
                    if (flag4) {
                        this.Index -= 3U;
                    }
                    else {
                        this.Index -= 6U;
                    }
                }
            }

       
            public void UpdateMatch() {
                this.Index = ((this.Index < 7U) ? 7U : 10U);
            }

 
            public void UpdateRep() {
                this.Index = ((this.Index < 7U) ? 8U : 11U);
            }


            public void UpdateShortRep() {
                this.Index = ((this.Index < 7U) ? 9U : 11U);
            }

         
            public bool IsCharState() {
                return this.Index < 7U;
            }

            public uint Index;
        }
    }
}
