using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.instruction
{
    public class ScopedInstruction : IInstruction
    {

        private List<IInstruction> instructions;

        public void setInstructions(List<IInstruction> ins) {
            instructions = ins;
        }

        public bool breakWasFlagged = false;

        public int execute(int current_instruction)
        {

            for (int i = 0; i < instructions.Count;) {


                int sum = instructions[i].execute(i);
               
                if (sum == 0) {
                    breakWasFlagged = true;
                    break;
                }
                i += sum;

            }

            return 1;

        }

        public override string ToString()
        {
            return "SCOPED INS; " + instructions.Count;
        }

        public void printInstructionTree(string prefix)
        {
            Console.WriteLine("_S");
            string prefix2 = prefix + "\t";
            foreach (IInstruction bl in instructions)
            {
                bl.printInstructionTree(prefix2);
            }
            Console.WriteLine("_E");
        }

        public string getFirstBlockDesc()
        {
            return "SCOPED";
        }
    }


}
