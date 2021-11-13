using System;
using System.Collections.Generic;
using System.Linq;
using de4dot.blocks;
using de4dot.blocks.cflow;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserKiller.Protections {
    internal class ControlFlow : BlockDeobfuscator {
    
        protected override bool Deobfuscate(Block block) {
            bool flag = false;
            bool flag2 = block.LastInstr.OpCode == OpCodes.Switch;
            bool flag3 = flag2;
            if (flag3) {
                this.allVars = this.blocks.Method.Body.Variables;
                this.isSwitchBlock(block);
                bool flag4 = this.switchBlock != null && this.localSwitch != null;
                bool flag5 = flag4;
                if (flag5) {
                    this.ins.Initialize(this.blocks.Method);
                    flag |= this.Cleaner();
                }
                this.isExpressionBlock(block);
                bool flag6 = this.switchBlock != null || this.localSwitch != null;
                bool flag7 = flag6;
                if (flag7) {
                    this.ins.Initialize(this.blocks.Method);
                    flag |= this.Cleaner();
                    while (this.Cleaner()) {
                        flag |= this.Cleaner();
                    }
                }
            }
            return flag;
        }


        private bool Cleaner() {
            bool result = false;
            List<Block> list = new List<Block>();
            foreach (Block block in this.allBlocks) {
                bool flag = block.FallThrough == this.switchBlock;
                bool flag2 = flag;
                if (flag2) {
                    list.Add(block);
                }
            }
            List<Block> list2 = new List<Block>();
            list2 = this.switchBlock.Targets;
            foreach (Block block2 in list) {
                bool flag3 = block2.LastInstr.IsLdcI4();
                bool flag4 = flag3;
                if (flag4) {
                    int ldcI4Value = block2.LastInstr.GetLdcI4Value();
                    this.ins.Push(new Int32Value(ldcI4Value));
                    int locVal;
                    int num = this.emulateCase(out locVal);
                    bool veryVerbose = Program.veryVerbose;
                    bool flag5 = veryVerbose;
                    if (flag5) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(num + ",");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    block2.ReplaceLastNonBranchWithBranch(0, list2[num]);
                    this.replace(list2[num], locVal);
                    block2.Instructions.Add(new Instr(new Instruction(OpCodes.Pop)));
                    result = true;
                }
                else {
                    bool flag6 = this.isXor(block2);
                    bool flag7 = flag6;
                    if (flag7) {
                        this.ins.Emulate(block2.Instructions, block2.Instructions.Count - 5, block2.Instructions.Count);
                        Int32Value value = (Int32Value)this.ins.Pop();
                        this.ins.Push(value);
                        int locVal2;
                        int num2 = this.emulateCase(out locVal2);
                        bool veryVerbose2 = Program.veryVerbose;
                        bool flag8 = veryVerbose2;
                        if (flag8) {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(num2 + ",");
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        block2.ReplaceLastNonBranchWithBranch(0, list2[num2]);
                        this.replace(list2[num2], locVal2);
                        block2.Instructions.Add(new Instr(new Instruction(OpCodes.Pop)));
                        result = true;
                    }
                    else {
                        bool flag9 = block2.Sources.Count == 2 && block2.Instructions.Count == 1;
                        bool flag10 = flag9;
                        if (flag10) {
                            List<Block> list3 = new List<Block>(block2.Sources);
                            foreach (Block block3 in list3) {
                                bool flag11 = block3.FirstInstr.IsLdcI4();
                                bool flag12 = flag11;
                                if (flag12) {
                                    int ldcI4Value2 = block3.FirstInstr.GetLdcI4Value();
                                    this.ins.Push(new Int32Value(ldcI4Value2));
                                    int locVal3;
                                    int num3 = this.emulateCase(out locVal3);
                                    bool veryVerbose3 = Program.veryVerbose;
                                    bool flag13 = veryVerbose3;
                                    if (flag13) {
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        bool flag14 = block3 == list3[0];
                                        bool flag15 = flag14;
                                        if (flag15) {
                                            Console.Write("True: " + num3 + ",");
                                        }
                                        else {
                                            Console.Write("False: " + num3 + ",");
                                        }
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    }
                                    block3.ReplaceLastNonBranchWithBranch(0, list2[num3]);
                                    this.replace(list2[num3], locVal3);
                                    block3.Instructions[1] = new Instr(new Instruction(OpCodes.Pop));
                                    result = true;
                                }
                            }
                        }
                        else {
                            bool flag16 = block2.LastInstr.OpCode == OpCodes.Xor;
                            bool flag17 = flag16;
                            if (flag17) {
                                bool flag18 = block2.Instructions[block2.Instructions.Count - 2].OpCode == OpCodes.Mul;
                                bool flag19 = flag18;
                                if (flag19) {
                                    List<Instr> instructions = block2.Instructions;
                                    int num4 = instructions.Count;
                                    bool flag20 = !instructions[num4 - 4].IsLdcI4();
                                    bool flag21 = !flag20;
                                    if (flag21) {
                                        List<Block> list4 = new List<Block>(block2.Sources);
                                        foreach (Block block4 in list4) {
                                            bool flag22 = block4.FirstInstr.IsLdcI4();
                                            bool flag23 = flag22;
                                            if (flag23) {
                                                int ldcI4Value3 = block4.FirstInstr.GetLdcI4Value();
                                                try {
                                                    instructions[num4 - 5] = new Instr(new Instruction(OpCodes.Ldc_I4, ldcI4Value3));
                                                }
                                                catch {
                                                    instructions.Insert(num4 - 4, new Instr(new Instruction(OpCodes.Ldc_I4, ldcI4Value3)));
                                                    num4++;
                                                }
                                                this.ins.Emulate(instructions, num4 - 5, num4);
                                                int locVal4;
                                                int num5 = this.emulateCase(out locVal4);
                                                bool veryVerbose4 = Program.veryVerbose;
                                                bool flag24 = veryVerbose4;
                                                if (flag24) {
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    bool flag25 = block4 == list4[0];
                                                    bool flag26 = flag25;
                                                    if (flag26) {
                                                        Console.Write("True: " + num5 + ",");
                                                    }
                                                    else {
                                                        Console.Write("False: " + num5 + ",");
                                                    }
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                }
                                                block4.ReplaceLastNonBranchWithBranch(0, list2[num5]);
                                                this.replace(list2[num5], locVal4);
                                                try {
                                                    block4.Instructions[1] = new Instr(new Instruction(OpCodes.Pop));
                                                }
                                                catch {
                                                    block4.Instructions.Add(new Instr(new Instruction(OpCodes.Pop)));
                                                }
                                                result = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

     
        private bool replace(Block test, int locVal) {
            bool flag = test.IsConditionalBranch();
            bool flag2 = flag;
            if (flag2) {
                bool flag3 = test.FallThrough.FallThrough == this.switchBlock;
                bool flag4 = flag3;
                if (flag4) {
                    test = test.FallThrough;
                }
                else {
                    test = test.FallThrough.FallThrough;
                }
            }
            bool flag5 = test.LastInstr.OpCode == OpCodes.Switch;
            bool flag6 = flag5;
            if (flag6) {
                test = test.FallThrough;
            }
            bool flag7 = test == this.switchBlock;
            bool flag8 = flag7;
            bool result;
            if (flag8) {
                result = false;
            }
            else {
                for (int i = 0; i < test.Instructions.Count; i++) {
                    bool flag9 = test.Instructions[i].Instruction.GetLocal(this.blocks.Method.Body.Variables) == this.localSwitch;
                    bool flag10 = flag9;
                    if (flag10) {
                        test.Instructions[i] = new Instr(Instruction.CreateLdcI4(locVal));
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

     
        public int emulateCase(out int localValueasInt) {
            this.ins.Emulate(this.switchBlock.Instructions, 0, this.switchBlock.Instructions.Count - 1);
            Int32Value int32Value = this.ins.GetLocal(this.localSwitch) as Int32Value;
            localValueasInt = int32Value.Value;
            return ((Int32Value)this.ins.Pop()).Value;
        }

 
        private bool isXor(Block block) {
            int num = block.Instructions.Count - 1;
            List<Instr> instructions = block.Instructions;
            bool flag = num < 4;
            bool flag2 = flag;
            bool result;
            if (flag2) {
                result = false;
            }
            else {
                bool flag3 = instructions[num].OpCode != OpCodes.Xor;
                bool flag4 = flag3;
                if (flag4) {
                    result = false;
                }
                else {
                    bool flag5 = !instructions[num - 1].IsLdcI4();
                    bool flag6 = flag5;
                    if (flag6) {
                        result = false;
                    }
                    else {
                        bool flag7 = instructions[num - 2].OpCode != OpCodes.Mul;
                        bool flag8 = flag7;
                        if (flag8) {
                            result = false;
                        }
                        else {
                            bool flag9 = !instructions[num - 3].IsLdcI4();
                            bool flag10 = flag9;
                            if (flag10) {
                                result = false;
                            }
                            else {
                                bool flag11 = !instructions[num - 4].IsLdcI4();
                                result = !flag11;
                            }
                        }
                    }
                }
            }
            return result;
        }

   
        private void isExpressionBlock(Block block) {
            bool flag = block.Instructions.Count < 7;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = !block.FirstInstr.IsStloc();
                bool flag4 = !flag3;
                if (flag4) {
                    this.switchBlock = block;
                    this.localSwitch = Instr.GetLocalVar(this.blocks.Method.Body.Variables.Locals, block.Instructions[block.Instructions.Count - 4]);
                }
            }
        }

      
        private void isNative(Block block) {
            bool flag = block.Instructions.Count <= 5;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = block.FirstInstr.OpCode != OpCodes.Call;
                bool flag4 = !flag3;
                if (flag4) {
                    this.switchBlock = block;
                    this.native = true;
                    this.localSwitch = Instr.GetLocalVar(this.allVars, block.Instructions[block.Instructions.Count - 4]);
                }
            }
        }


        private void isolderCflow(Block block) {
            bool flag = block.Instructions.Count <= 2;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = !block.FirstInstr.IsLdcI4();
                bool flag4 = !flag3;
                if (flag4) {
                    this.isolder = true;
                    this.switchBlock = block;
                }
            }
        }

      
        private void isolderNatCflow(Block block) {
            bool flag = block.Instructions.Count != 2;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = block.FirstInstr.OpCode != OpCodes.Call;
                bool flag4 = !flag3;
                if (flag4) {
                    this.isolder = true;
                    this.switchBlock = block;
                    this.native = true;
                }
            }
        }

     

        private void isolderExpCflow(Block block) {
            bool flag = block.Instructions.Count <= 2;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = !block.FirstInstr.IsStloc();
                bool flag4 = !flag3;
                if (flag4) {
                    this.isolder = true;
                    this.switchBlock = block;
                }
            }
        }

      

        private void isSwitchBlock(Block block) {
            bool flag = block.Instructions.Count <= 6;
            bool flag2 = !flag;
            if (flag2) {
                bool flag3 = !block.FirstInstr.IsLdcI4();
                bool flag4 = !flag3;
                if (flag4) {
                    this.switchBlock = block;
                    this.localSwitch = Instr.GetLocalVar(this.allVars, block.Instructions[block.Instructions.Count - 4]);
                }
            }
        }


        private Block switchBlock;

  
        private Local localSwitch;


        private bool native;

        
        private bool isolder;

        public MethodDef currentMethod;


        public InstructionEmulator ins = new InstructionEmulator();

     
        private IList<Local> allVars;
    }
}
