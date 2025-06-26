using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class PrintBlock : AbstractBlock, IExecutableBlock
    {
        Variable text;
        string variableName;
        public PrintBlock(string variableName)
        {
            this.variableName = variableName;
        }


        public override void execute()
        {
            this.text = Lch.instance.getVariable(variableName);
            if (text.variable == null)
            {
                Console.WriteLine("null");
            }
            else {
                Console.WriteLine(text.variable);
            }
            
        }

    }
}
