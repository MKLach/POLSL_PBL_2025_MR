using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.elements
{
    public interface IExecutableBlock
    {

        void execute();

        int execute_wrap(int i);

    }
}
