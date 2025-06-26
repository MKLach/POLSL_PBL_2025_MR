using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class NullAssignBlock : AbstractBlock
    {
        string leftHandVariableName; 

        public NullAssignBlock(string leftHandVariableName) {

            this.leftHandVariableName = leftHandVariableName;
        }

        public override void execute()
        {
            Variable left = Lch.instance.getVariable(leftHandVariableName);
            left.variable = null;

        }
    }
}
