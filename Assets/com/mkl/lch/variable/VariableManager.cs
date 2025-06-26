using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lch.com.mkl.lch.variable
{
    public interface VariableManager
    {
        Variable getVariable(string variableName);

        void addVariable(string name, Variable value);
        VariableMetadata getType(string type);
    }
}
