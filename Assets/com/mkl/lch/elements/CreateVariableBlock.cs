using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class CreateVariableBlock : AbstractBlock, IExecutableBlock
    {
        VariableManager manager;
        string variableName;
        Variable variable;
        public CreateVariableBlock(string name) : this(name, null, Lch.BUILTIN_INTEGER, Lch.instance)
        {
           
        }

        public CreateVariableBlock(string name, string right, byte[] type, VariableManager manager) {
            this.variableName = name;
            this.manager = manager;
        }

        public override void execute()
        {



        }

    }
}
