using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace ConfuserKiller.Protections {
    internal static class ResourcesDeobfuscator {
     
        internal static bool Deobfuscate(ModuleDefMD module) {
            MethodDef decrypterMethod = ResourcesDeobfuscator.GetDecrypterMethod(module);
            bool flag = decrypterMethod == null;
            bool result;
            if (flag) {
                result = false;
            }
            else {
                MethodDef methodDef = module.GlobalType.FindStaticConstructor();
                foreach (Instruction instruction in methodDef.Body.Instructions) {
                    bool flag2 = instruction.OpCode != OpCodes.Call || instruction.Operand as MethodDef != decrypterMethod;
                    if (!flag2) {
                        instruction.OpCode = OpCodes.Nop;
                        instruction.Operand = null;
                    }
                }
                ResourcesDeobfuscator.ModifyMethod(decrypterMethod);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    module.Write(memoryStream, new ModuleWriterOptions {
                        Logger = DummyLogger.NoThrowInstance
                    });
                    Assembly assembly = Assembly.Load(memoryStream.ToArray());
                    MethodBase methodBase = assembly.ManifestModule.ResolveMethod(decrypterMethod.MDToken.ToInt32());
                    byte[] data = (byte[])methodBase.Invoke(null, null);
                    ResourceCollection resources = ModuleDefMD.Load(data).Resources;
                    ResourcesDeobfuscator.totalResources = module.Resources.Count;
                    foreach (Resource resource in resources) {
                        bool flag3 = !module.Resources.Remove(module.Resources.Find(resource.Name));
                        if (!flag3) {
                            module.Resources.Add(resource);
                            ResourcesDeobfuscator.resourcesDecrypted++;
                        }
                    }
                    ResourcesDeobfuscator.RemoveMethod(decrypterMethod);
                }
                result = ResourcesDeobfuscator.totalResources > 0;
            }
            return result;
        }


        private static MethodDef GetDecrypterMethod(ModuleDef module) {
            MethodDef methodDef = module.GlobalType.FindStaticConstructor();
            IList<Instruction> instructions = methodDef.Body.Instructions;
            foreach (Instruction instruction in instructions) {
                bool flag = instruction.OpCode != OpCodes.Call;
                if (!flag) {
                    MethodDef methodDef2 = instruction.Operand as MethodDef;
                    bool flag2 = methodDef2 == null;
                    if (!flag2) {
                        bool flag3 = !methodDef2.DeclaringType.IsGlobalModuleType;
                        if (!flag3) {
                            bool flag4 = methodDef2.Attributes != (dnlib.DotNet.MethodAttributes.Private | dnlib.DotNet.MethodAttributes.FamANDAssem | dnlib.DotNet.MethodAttributes.Static | dnlib.DotNet.MethodAttributes.HideBySig);
                            if (!flag4) {
                                bool flag5 = methodDef2.ReturnType.ElementType != ElementType.Void;
                                if (!flag5) {
                                    bool hasParamDefs = methodDef2.HasParamDefs;
                                    if (!hasParamDefs) {
                                        bool flag6 = methodDef2.FindInstructionsNumber(OpCodes.Call, "System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)") != 1;
                                        if (!flag6) {
                                            FieldDef field = ResourcesDeobfuscator.FindAssemblyField(module);
                                            string operand = (from instr in methodDef2.Body.Instructions
                                                              where instr.OpCode == OpCodes.Stsfld
                                                              let fieldArray = instr.Operand as FieldDef
                                                              where fieldArray != null
                                                              where field == fieldArray
                                                              select instr.Operand.ToString()).FirstOrDefault<string>();
                                            bool flag7 = methodDef2.FindInstructionsNumber(OpCodes.Stsfld, operand) != 1;
                                            if (!flag7) {
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
            return null;
        }

        
        private static FieldDef FindAssemblyField(ModuleDef module) {
            foreach (FieldDef fieldDef in module.GlobalType.Fields) {
                bool flag = fieldDef.Attributes != (dnlib.DotNet.FieldAttributes.Private | dnlib.DotNet.FieldAttributes.FamANDAssem | dnlib.DotNet.FieldAttributes.Static);
                if (!flag) {
                    bool flag2 = !fieldDef.DeclaringType.IsGlobalModuleType;
                    if (!flag2) {
                        bool flag3 = fieldDef.FieldType.ElementType != ElementType.Class;
                        if (!flag3) {
                            bool flag4 = fieldDef.FieldType.FullName != "System.Reflection.Assembly";
                            if (!flag4) {
                                return fieldDef;
                            }
                        }
                    }
                }
            }
            return null;
        }

    
        private static void ModifyMethod(MethodDef method) {
            ITypeDefOrRef type = method.Module.Import(typeof(byte[]));
            method.ReturnType = type.ToTypeSig(true);
            IList<Instruction> instructions = method.Body.Instructions;
            foreach (Instruction instruction in instructions) {
                bool flag = instruction.OpCode != OpCodes.Call;
                if (!flag) {
                    string text = instruction.Operand.ToString();
                    bool flag2 = !text.Contains("System.Reflection.Assembly::Load(System.Byte[])");
                    if (!flag2) {
                        instruction.OpCode = OpCodes.Ret;
                        instruction.Operand = null;
                    }
                }
            }
        }

        private static void RemoveMethod(MethodDef method) {
            CilBody body = new CilBody {
                Instructions =
                {
                    Instruction.Create(OpCodes.Ldnull),
                    Instruction.Create(OpCodes.Ret)
                }
            };
            method.Body = body;
        }


        internal static int resourcesDecrypted;

        internal static int totalResources;
    }
}
