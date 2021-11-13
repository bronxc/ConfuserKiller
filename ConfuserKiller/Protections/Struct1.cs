using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserKiller.Protections {
    internal class Struct1 {

        internal Struct1(int int_1) {
            this.int_0 = int_1;
            this.struct0_0 = new Struct0[1 << int_1];
        }


        internal void method_0() {
            uint num = 1U;
            while ((ulong)num < 1UL << (this.int_0 & 31)) {
                this.struct0_0[(int)(uint)((UIntPtr)num)].method_0();
                num += 1U;
            }
        }


        internal uint method_1(Class0 class0_0) {
            uint num = 1U;
            for (int i = this.int_0; i > 0; i--) {
                num = (num << 1) + this.struct0_0[(int)(uint)((UIntPtr)num)].method_1(class0_0);
            }
            return num - (1U << this.int_0);
        }


        internal uint method_2(Class0 class0_0) {
            uint num = 1U;
            uint num2 = 0U;
            for (int i = 0; i < this.int_0; i++) {
                uint num3 = this.struct0_0[(int)(uint)((UIntPtr)num)].method_1(class0_0);
                num <<= 1;
                num += num3;
                num2 |= num3 << i;
            }
            return num2;
        }


        internal static uint smethod_0(Struct0[] struct0_1, uint uint_0, Class0 class0_0, int int_1) {
            uint num = 1U;
            uint num2 = 0U;
            for (int i = 0; i < int_1; i++) {
                uint num3 = struct0_1[(int)(uint)((UIntPtr)(uint_0 + num))].method_1(class0_0);
                num <<= 1;
                num += num3;
                num2 |= num3 << i;
            }
            return num2;
        }


        internal readonly Struct0[] struct0_0;


        internal readonly int int_0;
    }
}
