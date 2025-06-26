using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public class ConcatAssignBlock : AbstractBlock
    {
        string leftHandVariableName;
        List<string> variableNames;
        public ConcatAssignBlock(string leftHandVariableName, List<string> variableNames) {

            this.leftHandVariableName = leftHandVariableName;
            this.variableNames = variableNames;
        }


        public override void execute()
        {
            Variable left = Lch.instance.getVariable(leftHandVariableName);
            String copy = "";
           
            bool concatWithItself = false;
            if (variableNames.Contains(leftHandVariableName)) {
                concatWithItself = true;
                copy = (string)left.variable;
            } 

            
            left.variable = "";
            


            foreach (string vName in variableNames) {
                if (vName.Equals(leftHandVariableName)) {

                    left.variable += copy;
                    continue;
                }

                Variable right = Lch.instance.getVariable(vName);
                string raw = "";
                if (right.meta.getType() != "string")
                    raw = right.variable.ToString();
                else
                    raw = (string)right.variable;

                left.variable += raw;

            }
            
        }
    }
}
