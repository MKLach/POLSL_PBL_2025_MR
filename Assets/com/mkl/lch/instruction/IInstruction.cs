using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.instruction
{
    public interface IInstruction
    {
        int execute(int current_instruction);

        void printInstructionTree(string prefix);

        string getFirstBlockDesc();
    }
}
