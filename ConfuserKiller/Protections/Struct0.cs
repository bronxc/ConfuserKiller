using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserKiller.Protections {
    internal struct Struct0 {

        internal void method_0() {
            this.uint_0 = 1024U;
        }


        internal uint method_1(Class0 class0_0) {
            uint num = (class0_0.uint_1 >> 11) * this.uint_0;
            bool flag = class0_0.uint_0 < num;
            bool flag2 = flag;
            uint result;
            if (flag2) {
                class0_0.uint_1 = num;
                this.uint_0 += 2048U - this.uint_0 >> 5;
                bool flag3 = class0_0.uint_1 < 16777216U;
                bool flag4 = flag3;
                if (flag4) {
                    class0_0.uint_0 = (class0_0.uint_0 << 8) | (uint)((byte)class0_0.stream_0.ReadByte());
                    class0_0.uint_1 <<= 8;
                }
                result = 0U;
            }
            else {
                class0_0.uint_1 -= num;
                class0_0.uint_0 -= num;
                this.uint_0 -= this.uint_0 >> 5;
                bool flag5 = class0_0.uint_1 < 16777216U;
                bool flag6 = flag5;
                if (flag6) {
                    class0_0.uint_0 = (class0_0.uint_0 << 8) | (uint)((byte)class0_0.stream_0.ReadByte());
                    class0_0.uint_1 <<= 8;
                }
                result = 1U;
            }
            return result;
        }


        internal uint uint_0;
    }
}
