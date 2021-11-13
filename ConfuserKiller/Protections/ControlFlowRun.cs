using System;
using System.Collections.Generic;
using System.Linq;
using de4dot.blocks;
using de4dot.blocks.cflow;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class ControlFlowRun {
      
        public static void DeobfuscateCflow(MethodDef meth) {
            for (int i = 0; i < 2; i++) {
                ControlFlowRun.CfDeob = new BlocksCflowDeobfuscator();
                Blocks blocks = new Blocks(meth);
                List<Block> allBlocks = blocks.MethodBlocks.GetAllBlocks();
                blocks.RemoveDeadBlocks();
                blocks.RepartitionBlocks();
                blocks.UpdateBlocks();
                blocks.Method.Body.SimplifyBranches();
                blocks.Method.Body.OptimizeBranches();
                ControlFlowRun.CfDeob.Initialize(blocks);
                ControlFlowRun.CfDeob.Add(new ControlFlow());
                ControlFlowRun.CfDeob.Deobfuscate();
                blocks.RepartitionBlocks();
                IList<Instruction> instructions;
                IList<ExceptionHandler> exceptionHandlers;
                blocks.GetCode(out instructions, out exceptionHandlers);
                DotNetUtils.RestoreBody(meth, instructions, exceptionHandlers);
            }
        }

  
        public static bool hasCflow(MethodDef methods) {
            for (int i = 0; i < methods.Body.Instructions.Count; i++) {
                bool flag = methods.Body.Instructions[i].OpCode == OpCodes.Switch;
                if (flag) {
                    return true;
                }
            }
            return false;
        }

    
        public static void cleaner(ModuleDefMD module) {
            foreach (TypeDef typeDef in module.GetTypes()) {
                foreach (MethodDef methodDef in typeDef.Methods) {
                    bool flag = !methodDef.HasBody;
                    if (!flag) {
                        bool flag2 = ControlFlowRun.hasCflow(methodDef);
                        if (flag2) {
                            bool veryVerbose = Program.veryVerbose;
                            if (veryVerbose) {
                            }
                            ControlFlowRun.DeobfuscateCflow(methodDef);
                            bool veryVerbose2 = Program.veryVerbose;
                            if (veryVerbose2) {
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }

 
        private static BlocksCflowDeobfuscator CfDeob;
    }
}
