using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.elements;
using lch.com.mkl.lch.variable;
using com.mkl.lch.instruction;

namespace lch.com.mkl.lch.script
{
    public class ExecutableScript : VariableManager
    {

        public Dictionary<string, Variable> variables;
        public List<IInstruction> blocks;

        public ExecutableScript() {
            this.blocks = new List<IInstruction>();
        }

        public void addInstruction(IInstruction block) {
            this.blocks.Add(block);
        }

        public IInstruction getInstruction(int index) {
            return blocks[index];
        }

        public Variable getVariable(string variableName)
        {

            return variables[variableName];
        }

        public void addVariable(string name, Variable value)
        {
            variables.Add(name, value);

        }

        public VariableMetadata getType(string type) { return null; }

        public void printInstructionTree()
        {
            foreach (IInstruction bl in blocks)
            {
                bl.printInstructionTree("");
            }
        }

    }
}
