using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;
using com.mkl.lch.elements;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch;

namespace com.mkl.lch.elements
{
    public class IfBlock : AbstractBlock
    {
        LogicEvalBlock evalBlock;

        public IfBlock(LogicEvalBlock evalBlock) {
            this.evalBlock = evalBlock;


        }
        public override void execute() { }

        public override int execute_wrap(int current_instruction_index)
        {
            if (evalBlock.testTheCondition()) {
                return 0;
            }

            return 1;
        }
    }


    public class WhileBlock : AbstractBlock
    {
        LogicEvalBlock evalBlock;

        public WhileBlock(LogicEvalBlock evalBlock)
        {
            this.evalBlock = evalBlock;


        }
        public override void execute() { }

        public override int execute_wrap(int current_instruction_index)
        {
            if (evalBlock.testTheCondition())
            {
                return 0;
            }

            return 1;
        }

        public bool testTheCondition() {
            return evalBlock.testTheCondition();
        }

    }
}
