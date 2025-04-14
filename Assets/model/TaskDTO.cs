using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.model
{
    public class TaskDTO
    {
        public string shortTitle;
        public string title;
        public string description;
        public List<TaskDTO> subTasks; // For nested tasks

        public TaskDTO() { }

        public TaskDTO(string shortTitle, string title, string description)
        {
            this.title = title;
            this.shortTitle = shortTitle;
            this.description = description;
            this.subTasks = new List<TaskDTO>();
        }
    }
}
