using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.markdown
{
    public class MarkdownManager
    {
        public static string basePath = Application.persistentDataPath + "/checklists";
        public static void purgeAllChecklists() {
            if (!Directory.Exists(basePath))
            {
                Console.WriteLine("Directory not found: " + basePath);
                return;
            }
            
            string[] subfolders = Directory.GetDirectories(basePath);
            foreach (string folder in subfolders) {
                Directory.Delete(folder, true);
            }
               

        }

        public static string[] listAllChecklistGroups()
        {
            string[] subfolders = Directory.GetDirectories(basePath);
            
            return subfolders;

        }

        public static string[] listAllChecklists()
        {

            string[] mdFiles = Directory.GetFiles(basePath, "*.md", SearchOption.AllDirectories);


            return mdFiles;
        }

        public static bool AnyLocalChecklists() {
            try {
                return listAllChecklists().Length > 0;
            } catch (Exception e)
            {
                return false;
            }
            
        }


    }
}
