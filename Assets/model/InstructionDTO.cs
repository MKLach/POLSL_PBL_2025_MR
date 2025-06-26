using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.model
{
    public class InstructionDTO
    {
        public int id;
        public string shortTitle;
        public string title;
        

        public List<TaskDTO> tasks;

        public InstructionDTO() { }

        public InstructionDTO(string shortTitle, string title) {
            this.title = title;
            this.shortTitle = shortTitle;
            tasks = new List<TaskDTO>();
        }

        public InstructionDTO(InstructionDTO i) { 
            this.id = i.id;
            this.shortTitle = i.shortTitle;
            this.title = i.title;
            this.tasks = i.tasks;
        }
        

        public void addTask(string shortTitle, string description) { 
            tasks.Add(new TaskDTO(shortTitle, shortTitle, description));    
        }

    }
}
