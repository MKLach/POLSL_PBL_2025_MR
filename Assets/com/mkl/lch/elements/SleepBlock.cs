using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch;

namespace com.mkl.lch.elements
{
    public class SleepBlock : AbstractBlock, IExecutableBlock
    {
        Variable text;
        string variableName;
        public SleepBlock(string variableName)
        {
            this.variableName = variableName;
        }


        public override void execute()
        {

            text = Lch.instance.getVariable(variableName);


            Thread.Sleep((int)text.variable);
        }

    }
}
