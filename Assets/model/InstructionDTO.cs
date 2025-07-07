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
        public string iconName;

        public List<TaskDTO> tasks;

        public InstructionDTO() { }

        public InstructionDTO(string shortTitle, string icon) {
            this.iconName = icon;
            this.shortTitle = shortTitle;
            tasks = new List<TaskDTO>();
        }

        public void addTask(string shortTitle, string description) { 
            tasks.Add(new TaskDTO(shortTitle, shortTitle, description));    
        }

    }
}
