using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class AssignBlock : AbstractBlock
    {
        string leftHandVariableName; 
        string rightHandVariableName;
        public AssignBlock(string leftHandVariableName, string rightHandVariableName) {

            this.leftHandVariableName = leftHandVariableName;
            this.rightHandVariableName = rightHandVariableName;
        }


        public override void execute()
        {
            Variable left = Lch.instance.getVariable(leftHandVariableName);
            Variable right = Lch.instance.getVariable(rightHandVariableName);

            if (right.variable == null) {
                left.variable = null;
                return;
            }

            right.meta.clone(left, right);

        }
    }
}
