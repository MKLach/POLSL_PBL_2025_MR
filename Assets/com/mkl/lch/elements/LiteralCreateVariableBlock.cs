using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class LiteralCreateVariableBlock : AbstractBlock, IExecutableBlock
    {
        VariableManager manager;
        string variableName;
        Variable variable;
    

        public LiteralCreateVariableBlock(string name, object initial, string type, VariableManager manager) {
            this.variableName = name;
            this.variable = new Variable(name, initial, manager.getType(type));
            this.manager = manager;
        }

        public override void execute()
        {
           
            manager.addVariable(variableName, variable);

        }

        public override string ToString()
        {
            return "CVR; "+variableName;
        }

    }
}
