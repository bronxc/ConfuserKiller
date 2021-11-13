using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserKiller.Protections {
    internal class Class0 {
 
        internal void method_0(Stream stream_1) {
            this.stream_0 = stream_1;
            this.uint_0 = 0U;
            this.uint_1 = uint.MaxValue;
            for (int i = 0; i < 5; i++) {
                this.uint_0 = (this.uint_0 << 8) | (uint)((byte)this.stream_0.ReadByte());
            }
        }


        internal void method_1() {
            this.stream_0 = null;
        }


        internal void method_2() {
            while (this.uint_1 < 16777216U) {
                this.uint_0 = (this.uint_0 << 8) | (uint)((byte)this.stream_0.ReadByte());
                this.uint_1 <<= 8;
            }
        }


        internal uint method_3(int int_0) {
            uint num = this.uint_1;
            uint num2 = this.uint_0;
            uint num3 = 0U;
            for (int i = int_0; i > 0; i--) {
                num >>= 1;
                uint num4 = num2 - num >> 31;
                num2 -= num & (num4 - 1U);
                num3 = (num3 << 1) | (1U - num4);
                bool flag = num < 16777216U;
                bool flag2 = flag;
                if (flag2) {
                    num2 = (num2 << 8) | (uint)((byte)this.stream_0.ReadByte());
                    num <<= 8;
                }
            }
            this.uint_1 = num;
            this.uint_0 = num2;
            return num3;
        }


        internal Class0() {
        }


        internal uint uint_0;


        internal uint uint_1;

      
        internal Stream stream_0;
    }
}
