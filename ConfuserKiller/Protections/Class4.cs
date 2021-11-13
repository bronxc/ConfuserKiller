using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserKiller.Protections {
    internal class Class4 {
       
        internal void method_0(uint uint_3) {
            bool flag = this.uint_2 != uint_3;
            bool flag2 = flag;
            if (flag2) {
                this.byte_0 = new byte[uint_3];
            }
            this.uint_2 = uint_3;
            this.uint_0 = 0U;
            this.uint_1 = 0U;
        }

   
        internal void method_1(Stream stream_1, bool bool_0) {
            this.method_2();
            this.stream_0 = stream_1;
            bool flag = !bool_0;
            bool flag2 = flag;
            if (flag2) {
                this.uint_1 = 0U;
                this.uint_0 = 0U;
            }
        }

   
        internal void method_2() {
            this.method_3();
            this.stream_0 = null;
            Buffer.BlockCopy(new byte[this.byte_0.Length], 0, this.byte_0, 0, this.byte_0.Length);
        }

      
        internal void method_3() {
            uint num = this.uint_0 - this.uint_1;
            bool flag = num == 0U;
            bool flag2 = !flag;
            if (flag2) {
                this.stream_0.Write(this.byte_0, (int)this.uint_1, (int)num);
                bool flag3 = this.uint_0 >= this.uint_2;
                bool flag4 = flag3;
                if (flag4) {
                    this.uint_0 = 0U;
                }
                this.uint_1 = this.uint_0;
            }
        }

     
        internal void method_4(uint uint_3, uint uint_4) {
            uint num = this.uint_0 - uint_3 - 1U;
            bool flag = num >= this.uint_2;
            bool flag2 = flag;
            if (flag2) {
                num += this.uint_2;
            }
            while (uint_4 > 0U) {
                bool flag3 = num >= this.uint_2;
                bool flag4 = flag3;
                if (flag4) {
                    num = 0U;
                }
                byte[] array = this.byte_0;
                uint num2 = this.uint_0;
                this.uint_0 = num2 + 1U;
                array[(int)(uint)((UIntPtr)num2)] = this.byte_0[(int)(uint)((UIntPtr)(num++))];
                bool flag5 = this.uint_0 >= this.uint_2;
                bool flag6 = flag5;
                if (flag6) {
                    this.method_3();
                }
                uint_4 -= 1U;
            }
        }

     
        internal void method_5(byte byte_1) {
            byte[] array = this.byte_0;
            uint num = this.uint_0;
            this.uint_0 = num + 1U;
            array[(int)(uint)((UIntPtr)num)] = byte_1;
            bool flag = this.uint_0 >= this.uint_2;
            bool flag2 = flag;
            if (flag2) {
                this.method_3();
            }
        }

    
        internal byte method_6(uint uint_3) {
            uint num = this.uint_0 - uint_3 - 1U;
            bool flag = num >= this.uint_2;
            bool flag2 = flag;
            if (flag2) {
                num += this.uint_2;
            }
            return this.byte_0[(int)(uint)((UIntPtr)num)];
        }

     
        internal Class4() {
        }


        internal byte[] byte_0;


        internal uint uint_0;


        internal Stream stream_0;


        internal uint uint_1;


        internal uint uint_2;
    }
}
