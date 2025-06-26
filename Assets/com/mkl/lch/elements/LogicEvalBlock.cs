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
    public class LogicEvalBlock : AbstractBlock
    {
        string leftHandVariableName;
        List<string> variableNames;
        List<int> cons;
        public LogicEvalBlock(string leftHandVariableName, List<string> variableNames, List<int> cons) {

            this.leftHandVariableName = leftHandVariableName;
            this.variableNames = variableNames;
            this.cons = cons;
        }

        public bool testTheCondition() {
            Variable left = Lch.instance.getVariable(leftHandVariableName);

            bool previous = true;
            bool current = true;
            for (int i = 0; i < variableNames.Count; i++)
            {
                string vName = variableNames[i];
                Variable right = Lch.instance.getVariable(vName);

                //Console.WriteLine(right.name + " " + right.variable);


                if (right.meta == Lch.logicMeta)
                {
                    current = (bool)right.getVariable();
                }
                else if (right.meta == Lch.logicConvMeta)
                {
                    current = (bool)right.getVariable();
                }



                if ((cons[i] & 2) == 2)
                {
                    if ((cons[i] & 1) == 1)
                    {
                        current = !current || previous;
                    }
                    else
                    {
                        current = current || previous;
                    }


                }
                else if ((cons[i] & 4) == 4)
                {

                    if ((cons[i] & 1) == 1)
                    {
                        current = !current && previous;
                    }
                    else
                    {
                        current = current && previous;
                    }
                }
                else
                {

                    if ((cons[i] & 1) == 1)
                    {
                        current = !current;
                    }
                }


              


                previous = current;

            }

            left.variable = current;
            return current;

        }

        public override void execute()
        {
            testTheCondition();
        }
    }
}
