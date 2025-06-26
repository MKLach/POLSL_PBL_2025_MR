using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Assets.model;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;
using TMPro;
using System.Net.Http;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Meta.XR.ImmersiveDebugger.UserInterface;
using UnityEngine.Networking;
using Assets.JSONModel;
using System.IO;
using System.Net.NetworkInformation;
using Assets.Network;
using NUnit.Framework;
using Assets.Network.JSON;
using Oculus.Interaction;
using Unity.VisualScripting;
using Assets.Responses;
using static NetworkDataSingleton;
using Assets.markdown;

namespace Assets.InstructionProvider
{
    public class ExternalServerChecklistProvider : IChecklistProvider
    {
        //Dictionary<string, ChecklistGroup> checklists;
        string basePath = MarkdownManager.basePath;

        string serverUrl;
        NetworkDataSingleton local = NetworkDataSingleton.Instance;

        SavedChecklistProvider savedChecklistProvider;

        public ExternalServerChecklistProvider(string url) {
            serverUrl = url;
            //checklists = new Dictionary<string, ChecklistGroup>();
            savedChecklistProvider = new SavedChecklistProvider();
        }

        public Dictionary<string, ChecklistGroup> GetChecklists()
        {
            return savedChecklistProvider.GetChecklistsInternal();
        }

        bool ready = false;

        public bool isReady()
        {
            return savedChecklistProvider.isReady();
        }

        public IEnumerator signalDownload()
        {
            getAvaliablePlanes();
            Debug.Log("-----------------------------------------");
            savedChecklistProvider.load();
            Debug.Log("-----------------------------------------");
            yield return null;
        }

        void save(MDFileResponse resp)
        {

            string thepath = Path.Combine(basePath, resp.plane, resp.checklist_id + ".md");
            File.WriteAllText(thepath, resp.content);

        }

        void saveLCH(MDFileResponse resp)
        {
            if (resp.lch != "NONE") {
                string thepath = Path.Combine(basePath, resp.plane, resp.checklist_id + ".lch");
                File.WriteAllText(thepath, resp.lch);
            }
            

        }

        void DirectoryLogic(string thepath) {

            if (!Directory.Exists(thepath))
            {
                Directory.CreateDirectory(thepath);
                local.Enqueue(new Print("Created " + thepath));
            }
            else
            {
                local.Enqueue(new Print("Directory " + thepath + " already exists"));
            }

        }

        void processChecklists(ChecklistPerPlaneDataResponse avch)
        {
            local.Enqueue(new Print("Got " + avch.checklist_titles.Count + " checklists"));


            foreach (string ch_id in avch.checklist_titles)
            {
                local.Enqueue(new Print("Fetching " + ch_id + ""));

                MDFileResponse resp = getMdFile(avch.plane, ch_id);

                try
                {
                    MarkdownCompiler.Compile(resp.content);
                    save(resp);
                    saveLCH(resp);

                } catch (Exception e)
                {
                    local.Enqueue(new Print("Compilation failed " + ch_id + ""));

                }

            }
        }

        void getAvaliablePlanes()
        {
            PlaneResponse pr = getAvaliableChecklists();
            

            
            local.Enqueue(new Print("Fetched " + pr.planes.Count + ".")); 
            local.Enqueue(new Print("Persistent Dir: " + basePath));

            DirectoryLogic(basePath);

            foreach (string plane in pr.planes) {

                string thepath = Path.Combine(basePath, plane);
                local.Enqueue(new Print("Processing... " + plane));
                
                DirectoryLogic(thepath);

                ChecklistPerPlaneDataResponse avch = getChecklists(plane);


                processChecklists(avch);

            }

        }

        ChecklistPerPlaneDataResponse getChecklists(string plane) {
            string url = serverUrl + "/checklist/" + plane;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            Debug.Log("req " + url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Debug.Log("respc " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string resp = reader.ReadToEnd();
                    return JsonUtility.FromJson<ChecklistPerPlaneDataResponse>(resp);

                }

            }
        }

        PlaneResponse getAvaliableChecklists() {

            string url = serverUrl + "/checklist";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            Debug.Log("req " + url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Debug.Log("respc " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string resp = reader.ReadToEnd();

                    return JsonUtility.FromJson<PlaneResponse>(resp);

                }

            }
        }

        MDFileResponse getMdFile(string plane, string name)
        {

            string url = serverUrl + "/checklist/" + plane + "/"+name;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            Debug.Log("req " + url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Debug.Log("respc " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string resp = reader.ReadToEnd();

                    return JsonUtility.FromJson<MDFileResponse>(resp);

                }

            }
        }
    }
}
