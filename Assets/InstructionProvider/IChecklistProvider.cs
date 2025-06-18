using Assets.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.InstructionProvider
{
    public interface IChecklistProvider
    {
        public Dictionary<string, ChecklistGroup> GetChecklists();
        public void signalDownload();

        public bool isReady();
    }
}
