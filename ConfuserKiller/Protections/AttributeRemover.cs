using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace ConfuserKiller.Protections {
    internal class AttributeRemover {
      
        private static IList<TypeDef> lista(ModuleDef A_0) {
            return A_0.Types;
        }

      
        public static int start(ModuleDefMD md) {
            int num = 0;
            foreach (TypeDef typeDef in md.GetTypes()) {
                bool flag = typeDef.Name == "ConfusedByAttribute";
                bool flag2 = flag;
                if (flag2) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag3 = typeDef.Name == "ZYXDNGuarder";
                bool flag4 = flag3;
                if (flag4) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag5 = typeDef.Name == "YanoAttribute";
                bool flag6 = flag5;
                if (flag6) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag7 = typeDef.Name == "Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode";
                bool flag8 = flag7;
                if (flag8) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag9 = typeDef.Name == "SmartAssembly.Attributes.PoweredByAttribute";
                bool flag10 = flag9;
                if (flag10) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag11 = typeDef.Name == "SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute";
                bool flag12 = flag11;
                if (flag12) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag13 = typeDef.Name == "ObfuscatedByGoliath";
                bool flag14 = flag13;
                if (flag14) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag15 = typeDef.Name == "NineRays.Obfuscator.Evaluation";
                bool flag16 = flag15;
                if (flag16) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag17 = typeDef.Name == "EMyPID_8234_";
                bool flag18 = flag17;
                if (flag18) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag19 = typeDef.Name == "DotfuscatorAttribute";
                bool flag20 = flag19;
                if (flag20) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag21 = typeDef.Name == "CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute";
                bool flag22 = flag21;
                if (flag22) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag23 = typeDef.Name == "BabelObfuscatorAttribute";
                bool flag24 = flag23;
                if (flag24) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag25 = typeDef.Name == ".NETGuard";
                bool flag26 = flag25;
                if (flag26) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag27 = typeDef.Name == "OrangeHeapAttribute";
                bool flag28 = flag27;
                if (flag28) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag29 = typeDef.Name == "WTF";
                bool flag30 = flag29;
                if (flag30) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
                bool flag31 = typeDef.Name == "<ObfuscatedByDotNetPatcher>";
                bool flag32 = flag31;
                if (flag32) {
                    AttributeRemover.lista(md).Remove(typeDef);
                    num++;
                }
            }
            return num;
        }
    }
}
