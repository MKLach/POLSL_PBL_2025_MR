using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class LiteralAssignBlock : AbstractBlock
    {
        string leftHandVariableName; 
        object right;
        public LiteralAssignBlock(string leftHandVariableName, object literal) {

            this.leftHandVariableName = leftHandVariableName;
            this.right = literal;
        }


        public override void execute()
        {
            Variable left = Lch.instance.getVariable(leftHandVariableName);
            left.variable = right;

        }
    }
}
