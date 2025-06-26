using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkl.lch.elements;

namespace com.mkl.lch.instruction
{
    public class Instruction : IInstruction
    {
        protected List<IExecutableBlock> blocks;

        public Instruction(List<IExecutableBlock> blocks) {
            this.blocks = blocks;
        }

        public virtual int execute(int current_instruction) {
            int sum = 0;
            foreach(IExecutableBlock block in blocks) {

                sum+=block.execute_wrap(current_instruction);
                
            }
          
            return sum + 1;
        }


        public override string ToString()
        {
            return "INS; " + blocks.Count;
        }

        public virtual void printInstructionTree(string prefix) {
            Console.WriteLine("_I");
            foreach (IExecutableBlock bl in blocks) {
                Console.WriteLine(prefix + bl.GetType());
            }
        }

        public string getFirstBlockDesc()
        {
            return blocks[blocks.Count-1].GetType().ToString();
        }
    }

    public class WhileInstruction : Instruction {

        ScopedInstruction scope;


        WhileBlock core;

        public WhileInstruction(List<IExecutableBlock> blocks) : base(blocks)
        {

            core = (WhileBlock) blocks.Last();
            blocks.Remove(core);
        }

        public void setNestedScope(IInstruction scope) {
            this.scope = (ScopedInstruction)scope;
        }

        public override int execute(int current_instruction)
        {
            int sum = 0;
            foreach (IExecutableBlock block in blocks)
            {

                sum += block.execute_wrap(current_instruction);

            }

            while (core.testTheCondition() && !scope.breakWasFlagged) {

                int sum2 = scope.execute(0);

            }

            return sum + 1;
        }
        public override void printInstructionTree(string prefix)
        {
            Console.WriteLine("_W");
            foreach (IExecutableBlock bl in blocks)
            {
                Console.WriteLine(prefix + bl.GetType());
            }

            scope.printInstructionTree(prefix + "\t");

        }
    }


}
