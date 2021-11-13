using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserKiller.Protections {
    internal struct Struct3 {

        internal void method_0() {
            this.uint_0 = 0U;
        }


        internal void method_1() {
            bool flag = this.uint_0 < 4U;
            bool flag2 = flag;
            if (flag2) {
                this.uint_0 = 0U;
            }
            else {
                bool flag3 = this.uint_0 < 10U;
                bool flag4 = flag3;
                if (flag4) {
                    this.uint_0 -= 3U;
                }
                else {
                    this.uint_0 -= 6U;
                }
            }
        }


        internal void method_2() {
            this.uint_0 = ((this.uint_0 < 7U) ? 7U : 10U);
        }

       
        internal void method_3() {
            this.uint_0 = ((this.uint_0 < 7U) ? 8U : 11U);
        }

        internal void method_4() {
            this.uint_0 = ((this.uint_0 < 7U) ? 9U : 11U);
        }

      
        internal bool method_5() {
            return this.uint_0 < 7U;
        }


        internal uint uint_0;
    }
}
