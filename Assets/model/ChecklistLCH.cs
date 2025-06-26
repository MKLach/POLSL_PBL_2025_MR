using lch.com.mkl.lch.script;
using lch.com.mkl.lch.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.model
{
    public class ChecklistLCH : InstructionDTO
    {
        public string[] lch = null;
        public ExecutableScript es;
        public ChecklistLCH(string[] lch) { 
            this.lch = lch;

        }

        public ChecklistLCH(string[] lch, InstructionDTO dot) : base(dot)
        {
            this.lch = lch;
            this.title = dot.title + " lch";
            this.shortTitle = dot.shortTitle + " lch";
            
        }

        public void replaceTask(Variable index, Variable title, Variable desc) {

            this.tasks[index.getAsInt()].title = this.tasks[index.getAsInt()].shortTitle = title.getAsString();
            this.tasks[index.getAsInt()].description = desc.getAsString();


        }

        public void addTask(Variable title, Variable desc, Variable reserved)
        {
            
            this.addTask(title.getAsString(), desc.getAsString());

        }

        public void clear()
        {
            this.tasks.Clear();

        }
    }
}
