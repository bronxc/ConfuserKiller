using System;
using System.IO;

namespace ConfuserKiller.Protections {
    internal class Class1 {
  
        internal Class1() {
            this.uint_0 = uint.MaxValue;
            int num = 0;
            while ((long)num < 4L) {
                this.struct1_0[num] = new Struct1(6);
                num++;
            }
        }


        internal void method_0(uint uint_3) {
            bool flag = this.uint_0 != uint_3;
            bool flag2 = flag;
            if (flag2) {
                this.uint_0 = uint_3;
                this.uint_1 = Math.Max(this.uint_0, 1U);
                uint uint_4 = Math.Max(this.uint_1, 4096U);
                this.class4_0.method_0(uint_4);
            }
        }


        internal void method_1(int int_0, int int_1) {
            this.class3_0.method_0(int_0, int_1);
        }

      
        internal void method_2(int int_0) {
            uint num = 1U << int_0;
            this.class2_0.method_0(num);
            this.class2_1.method_0(num);
            this.uint_2 = num - 1U;
        }


        internal void method_3(Stream stream_0, Stream stream_1) {
            this.class0_0.method_0(stream_0);
            this.class4_0.method_1(stream_1, this.bool_0);
            for (uint num = 0U; num < 12U; num += 1U) {
                for (uint num2 = 0U; num2 <= this.uint_2; num2 += 1U) {
                    uint value = (num << 4) + num2;
                    this.struct0_0[(int)(uint)((UIntPtr)value)].method_0();
                    this.struct0_1[(int)(uint)((UIntPtr)value)].method_0();
                }
                this.struct0_2[(int)(uint)((UIntPtr)num)].method_0();
                this.struct0_3[(int)(uint)((UIntPtr)num)].method_0();
                this.struct0_4[(int)(uint)((UIntPtr)num)].method_0();
                this.struct0_5[(int)(uint)((UIntPtr)num)].method_0();
            }
            this.class3_0.method_1();
            for (uint num3 = 0U; num3 < 4U; num3 += 1U) {
                this.struct1_0[(int)(uint)((UIntPtr)num3)].method_0();
            }
            for (uint num4 = 0U; num4 < 114U; num4 += 1U) {
                this.struct0_6[(int)(uint)((UIntPtr)num4)].method_0();
            }
            this.class2_0.method_1();
            this.class2_1.method_1();
            this.struct1_1.method_0();
        }

   
        internal void method_4(Stream stream_0, Stream stream_1, long long_0, long long_1) {
            this.method_3(stream_0, stream_1);
            Struct3 @struct = default(Struct3);
            @struct.method_0();
            uint num = 0U;
            uint num2 = 0U;
            uint num3 = 0U;
            uint num4 = 0U;
            ulong num5 = 0UL;
            bool flag = 0L < long_1;
            bool flag2 = flag;
            if (flag2) {
                this.struct0_0[(int)(uint)((UIntPtr)(@struct.uint_0 << 4))].method_1(this.class0_0);
                @struct.method_1();
                byte byte_ = this.class3_0.method_3(this.class0_0, 0U, 0);
                this.class4_0.method_5(byte_);
                num5 += 1UL;
            }
            while (num5 < (ulong)long_1) {
                uint num6 = (uint)num5 & this.uint_2;
                bool flag3 = this.struct0_0[(int)(uint)((UIntPtr)((@struct.uint_0 << 4) + num6))].method_1(this.class0_0) == 0U;
                bool flag4 = flag3;
                if (flag4) {
                    byte byte_2 = this.class4_0.method_6(0U);
                    bool flag5 = !@struct.method_5();
                    bool flag6 = flag5;
                    byte byte_3;
                    if (flag6) {
                        byte_3 = this.class3_0.method_4(this.class0_0, (uint)num5, byte_2, this.class4_0.method_6(num));
                    }
                    else {
                        byte_3 = this.class3_0.method_3(this.class0_0, (uint)num5, byte_2);
                    }
                    this.class4_0.method_5(byte_3);
                    @struct.method_1();
                    num5 += 1UL;
                }
                else {
                    bool flag7 = this.struct0_2[(int)(uint)((UIntPtr)@struct.uint_0)].method_1(this.class0_0) == 1U;
                    bool flag8 = flag7;
                    uint num8;
                    if (flag8) {
                        bool flag9 = this.struct0_3[(int)(uint)((UIntPtr)@struct.uint_0)].method_1(this.class0_0) == 0U;
                        bool flag10 = flag9;
                        if (flag10) {
                            bool flag11 = this.struct0_1[(int)(uint)((UIntPtr)((@struct.uint_0 << 4) + num6))].method_1(this.class0_0) == 0U;
                            bool flag12 = flag11;
                            if (flag12) {
                                @struct.method_4();
                                this.class4_0.method_5(this.class4_0.method_6(num));
                                num5 += 1UL;
                                continue;
                            }
                        }
                        else {
                            bool flag13 = this.struct0_4[(int)(uint)((UIntPtr)@struct.uint_0)].method_1(this.class0_0) == 0U;
                            bool flag14 = flag13;
                            uint num7;
                            if (flag14) {
                                num7 = num2;
                            }
                            else {
                                bool flag15 = this.struct0_5[(int)(uint)((UIntPtr)@struct.uint_0)].method_1(this.class0_0) == 0U;
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
                        num8 = this.class2_1.method_2(this.class0_0, num6) + 2U;
                        @struct.method_3();
                    }
                    else {
                        num4 = num3;
                        num3 = num2;
                        num2 = num;
                        num8 = 2U + this.class2_0.method_2(this.class0_0, num6);
                        @struct.method_2();
                        uint num9 = this.struct1_0[(int)(uint)((UIntPtr)Class1.smethod_0(num8))].method_1(this.class0_0);
                        bool flag17 = num9 >= 4U;
                        bool flag18 = flag17;
                        if (flag18) {
                            int num10 = (int)((num9 >> 1) - 1U);
                            num = (2U | (num9 & 1U)) << num10;
                            bool flag19 = num9 < 14U;
                            bool flag20 = flag19;
                            if (flag20) {
                                num += Struct1.smethod_0(this.struct0_6, num - num9 - 1U, this.class0_0, num10);
                            }
                            else {
                                num += this.class0_0.method_3(num10 - 4) << 4;
                                num += this.struct1_1.method_2(this.class0_0);
                            }
                        }
                        else {
                            num = num9;
                        }
                    }
                    bool flag21 = ((ulong)num >= num5 || num >= this.uint_1) && num == uint.MaxValue;
                    bool flag22 = flag21;
                    if (flag22) {
                        break;
                    }
                    this.class4_0.method_4(num, num8);
                    num5 += (ulong)num8;
                }
            }
            this.class4_0.method_3();
            this.class4_0.method_2();
            this.class0_0.method_1();
        }

        internal void method_5(byte[] byte_0) {
            int int_ = (int)(byte_0[0] % 9);
            int num = (int)(byte_0[0] / 9);
            int int_2 = num % 5;
            int int_3 = num / 5;
            uint num2 = 0U;
            for (int i = 0; i < 4; i++) {
                num2 += (uint)((uint)byte_0[1 + i] << i * 8);
            }
            this.method_0(num2);
            this.method_1(int_2, int_);
            this.method_2(int_3);
        }

   
        internal void method_01(uint uint_3) {
            bool flag = this.uint_0 != uint_3;
            bool flag2 = flag;
            if (flag2) {
                this.uint_0 = uint_3;
                this.uint_1 = Math.Max(this.uint_0, 1U);
                uint uint_4 = Math.Max(this.uint_1, 4096U);
                this.class4_0.method_0(uint_4);
            }
        }


        internal static uint smethod_0(uint uint_3) {
            uint_3 -= 2U;
            bool flag = uint_3 < 4U;
            bool flag2 = flag;
            uint result;
            if (flag2) {
                result = uint_3;
            }
            else {
                result = 3U;
            }
            return result;
        }


        internal readonly Struct0[] struct0_0 = new Struct0[192];


        internal readonly Struct0[] struct0_1 = new Struct0[192];

        internal readonly Struct0[] struct0_2 = new Struct0[12];


        internal readonly Struct0[] struct0_3 = new Struct0[12];


        internal readonly Struct0[] struct0_4 = new Struct0[12];


        internal readonly Struct0[] struct0_5 = new Struct0[12];


        internal readonly Class1.Class2 class2_0 = new Class1.Class2();


        internal readonly Class1.Class3 class3_0 = new Class1.Class3();

        internal readonly Class4 class4_0 = new Class4();


        internal readonly Struct0[] struct0_6 = new Struct0[114];


        internal readonly Struct1[] struct1_0 = new Struct1[4];

          internal readonly Class0 class0_0 = new Class0();


        internal readonly Class1.Class2 class2_1 = new Class1.Class2();


        internal bool bool_0;


        internal uint uint_0;


        internal uint uint_1;

        internal Struct1 struct1_1 = new Struct1(4);

        internal uint uint_2;


        internal class Class2 {

            internal void method_0(uint uint_1) {
                for (uint num = this.uint_0; num < uint_1; num += 1U) {
                    this.struct1_0[(int)(uint)((UIntPtr)num)] = new Struct1(3);
                    this.struct1_1[(int)(uint)((UIntPtr)num)] = new Struct1(3);
                }
                this.uint_0 = uint_1;
            }


            internal void method_1() {
                this.struct0_0.method_0();
                for (uint num = 0U; num < this.uint_0; num += 1U) {
                    this.struct1_0[(int)(uint)((UIntPtr)num)].method_0();
                    this.struct1_1[(int)(uint)((UIntPtr)num)].method_0();
                }
                this.struct0_1.method_0();
                this.struct1_2.method_0();
            }


            internal uint method_2(Class0 class0_0, uint uint_1) {
                bool flag = this.struct0_0.method_1(class0_0) == 0U;
                bool flag2 = flag;
                uint result;
                if (flag2) {
                    result = this.struct1_0[(int)(uint)((UIntPtr)uint_1)].method_1(class0_0);
                }
                else {
                    uint num = 8U;
                    bool flag3 = this.struct0_1.method_1(class0_0) == 0U;
                    bool flag4 = flag3;
                    if (flag4) {
                        num += this.struct1_1[(int)(uint)((UIntPtr)uint_1)].method_1(class0_0);
                    }
                    else {
                        num += 8U;
                        num += this.struct1_2.method_1(class0_0);
                    }
                    result = num;
                }
                return result;
            }

            internal Class2() {
            }


            internal readonly Struct1[] struct1_0 = new Struct1[16];


            internal readonly Struct1[] struct1_1 = new Struct1[16];

   
            internal Struct0 struct0_0 = default(Struct0);

            
            internal Struct0 struct0_1 = default(Struct0);


            internal Struct1 struct1_2 = new Struct1(8);

            internal uint uint_0;
        }

      
        internal class Class3 {

            internal void method_0(int int_2, int int_3) {
                bool flag = this.struct2_0 != null && this.int_1 == int_3 && this.int_0 == int_2;
                bool flag2 = !flag;
                if (flag2) {
                    this.int_0 = int_2;
                    this.uint_0 = (1U << int_2) - 1U;
                    this.int_1 = int_3;
                    uint num = 1U << this.int_1 + this.int_0;
                    this.struct2_0 = new Class1.Class3.Struct2[num];
                    for (uint num2 = 0U; num2 < num; num2 += 1U) {
                        this.struct2_0[(int)(uint)((UIntPtr)num2)].method_0();
                    }
                }
            }

      
            internal void method_1() {
                uint num = 1U << this.int_1 + this.int_0;
                for (uint num2 = 0U; num2 < num; num2 += 1U) {
                    this.struct2_0[(int)(uint)((UIntPtr)num2)].method_1();
                }
            }

            internal uint method_2(uint uint_1, byte byte_0) {
                return ((uint_1 & this.uint_0) << this.int_1) + (uint)(byte_0 >> 8 - this.int_1);
            }

   
            internal byte method_3(Class0 class0_0, uint uint_1, byte byte_0) {
                return this.struct2_0[(int)(uint)((UIntPtr)this.method_2(uint_1, byte_0))].method_2(class0_0);
            }


            internal byte method_4(Class0 class0_0, uint uint_1, byte byte_0, byte byte_1) {
                return this.struct2_0[(int)(uint)((UIntPtr)this.method_2(uint_1, byte_0))].method_3(class0_0, byte_1);
            }

           
            internal Class3() {
            }

        
            internal Class1.Class3.Struct2[] struct2_0;

           
            internal int int_0;


            internal int int_1;

         
            internal uint uint_0;

  
            internal struct Struct2 {
           
                internal void method_0() {
                    this.struct0_0 = new Struct0[768];
                }

     
                internal void method_1() {
                    for (int i = 0; i < 768; i++) {
                        this.struct0_0[i].method_0();
                    }
                }

              
                internal byte method_2(Class0 class0_0) {
                    uint num = 1U;
                    do {
                        num = (num << 1) | this.struct0_0[(int)(uint)((UIntPtr)num)].method_1(class0_0);
                    }
                    while (num < 256U);
                    return (byte)num;
                }

        
                internal byte method_3(Class0 class0_0, byte byte_0) {
                    uint num = 1U;
                    for (; ; )
                    {
                        uint num2 = (uint)((byte_0 >> 7) & 1);
                        byte_0 = (byte)(byte_0 << 1);
                        uint num3 = this.struct0_0[(int)(uint)((UIntPtr)((1U + num2 << 8) + num))].method_1(class0_0);
                        num = (num << 1) | num3;
                        bool flag = num2 != num3;
                        bool flag2 = flag;
                        if (flag2) {
                            break;
                        }
                        bool flag3 = num >= 256U;
                        bool flag4 = flag3;
                        if (flag4) {
                            goto Block_2;
                        }
                    }
                    while (num < 256U) {
                        num = (num << 1) | this.struct0_0[(int)(uint)((UIntPtr)num)].method_1(class0_0);
                    }
                    Block_2:
                    return (byte)num;
                }

              
                internal Struct0[] struct0_0;
            }
        }
    }
}
