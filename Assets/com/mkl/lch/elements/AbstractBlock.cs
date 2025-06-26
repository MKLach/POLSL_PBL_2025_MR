using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkl.lch.elements;

namespace lch.com.mkl.lch.elements
{
    public abstract class AbstractBlock : IExecutableBlock
    {
        public abstract void execute() ;
        protected string keyword { get; set; }

        public virtual int execute_wrap(int current_instruction_index) {

            execute();

            return 0;
        }
    }
}
