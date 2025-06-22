using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Threading;
using Assets.markdown;
using System.IO;
using static NetworkDataSingleton;
using UnityEngine.InputSystem.HID;

namespace Assets.InstructionProvider
{
    public class SavedChecklistProvider : IChecklistProvider
    {
        Dictionary<string, ChecklistGroup> checklists;
        Dictionary<string, ChecklistGroup> IChecklistProvider.GetChecklists()
        {
            return checklists;
        }

        public Dictionary<string, ChecklistGroup> GetChecklistsInternal()
        {
            return checklists;
        }


        bool ready = false;
        NetworkDataSingleton local = NetworkDataSingleton.Instance;


        IEnumerator IChecklistProvider.signalDownload()
        {

            load();

            
            yield return null;
        }

        public void load() {
            checklists = new Dictionary<string, ChecklistGroup>();

            string[] chs = MarkdownManager.listAllChecklistGroups();
            Debug.Log("AVA CHK");
            local.Enqueue(new Print("ACA CHK " + chs.Length));
            foreach (string ch in chs)
            {
                string[] mdFiles = Directory.GetFiles(ch, "*.md", SearchOption.AllDirectories);

                ChecklistGroup chgrp = new ChecklistGroup();

                foreach (string mdFile in mdFiles)
                {
                    string content = File.ReadAllText(mdFile); ;
                    try { 
                        InstructionDTO instructionDTO = MarkdownCompiler.Compile(content);
                        chgrp.instructions.Add(instructionDTO.id, instructionDTO);
                    } catch (Exception ex)
                    {

                    }
                }

                if(chgrp.instructions.Count > 0)
                {
                   
                    string lastFolder = Path.GetFileName(ch.TrimEnd('/', '\\'));
                    checklists.Add(lastFolder, chgrp);

                }
                

            }
            ready = true;

        }



        public bool isReady()
        {

            return ready;
        }

        public bool isReadyInternal()
        {

            return ready;
        }

    }
}
