using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch;
using com.mkl.lch.elements;

namespace com.mkl.lch.elements
{
    public class RelativeJumpBlock : AbstractBlock, IExecutableBlock
    {
        Variable text;
        string variableName;
        public RelativeJumpBlock(string variableName)
        {
            this.variableName = variableName;
        }


        public override void execute()
        {

        }

        public override int execute_wrap(int current_instruction_index)
        {
            text = Lch.instance.getVariable(variableName);

            return (int)text.variable;
        }


    }
}
