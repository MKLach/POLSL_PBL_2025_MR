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
using Assets.markdown;

public class NetworkInformationSearcher : MonoBehaviour
{

    public TextMeshProUGUI console;
    volatile bool found = false;
    volatile String temp = "";
    public string serverURL = null;
    public string ip = "192.168.100.114";
    public string baseIp = "192.168.100.";
    public string netmask = "255.255.255.0";

    

    public string currentUser = null;

    public GameObject listPanelPrefab;

    public GameObject pageContainerParent;

    //bool started = false;
    bool done = false;
    volatile string jsonResponse = null;
    float start;

    List<UnicastIPAddressInformation> ips = new List<UnicastIPAddressInformation>();

    public UnicastIPAddressInformation theDeviceIpAddress;

    List<IPAddress> addresses;

    public void onSelectChache(GameObject list, int chosenIndex) {

        if (chosenIndex == 1)
        {
            console.text += "no -> scan option\n";
            startScan();
        }
        else {
            console.text += "attemping to connect...\n";
            string url = cached.url + "/discovery";
            string url1 = cached.url;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                Debug.Log("req " + url);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Debug.Log($"Found: {url} - Status: {(int)response.StatusCode}");
                    found = true;
                    temp = url1;
                    getUser = true;

                }
            }
            catch (WebException)
            {
                console.text += "failure, starting scan anyway...\n";
                startScan();


            }
        }


        Destroy(list);
    }

    int cached_list_usable = 0;
    int safemode_off = 0;

    BaseIpToUrl cached;
    public void onChosenCallback(GameObject list, int chosenIndex) {

        if (chosenIndex >= this.ips.Count) {
            if (chosenIndex == this.ips.Count + safemode_off) {
                console.text += "Selected Safe Mode...\n";
                useSafeModeUser();

                Destroy(list);
                return;
            }

            if (chosenIndex == this.ips.Count)
            {
                console.text += "Selected Cached Mode...\n";
                useLocalUser();

                Destroy(list);
                return;
            }

        }

        selectInterface(ips[chosenIndex]);
        Destroy(list);

        cached = cached_urls.get(this.baseIp);

        if (cached != null)
        {

            console.text += "Cached ips contains entry for this subnet:\n" + cached.url + "\n";
            GameObject panel1 = Instantiate(listPanelPrefab, pageContainerParent.transform);

            panel1.name = "chosev1";
            ListPanelController lpc = panel1.GetComponent<ListPanelController>();
            lpc.setTitle("connect ->  " + cached.url);
            lpc.addOption("Yes");
            lpc.addOption("Scan anyway");

            lpc.onOptionChosen = onSelectChache;

        }
        else
        {
            startScan();
        }
    }

    public void selectInterface(UnicastIPAddressInformation address) {
        theDeviceIpAddress = address;
        this.ip = theDeviceIpAddress.Address.ToString();
        this.netmask = theDeviceIpAddress.IPv4Mask.ToString();
        this.baseIp = GetBaseIP(this.ip, this.netmask);

        console.text += "Device IP: " + this.ip + "\n";
        console.text += "Base IP: " + this.baseIp + "\n";
        console.text += "Target Port: " + port + "\n";

        addresses = SubnetIPsRetriever.GetAllIPsInSubnet(theDeviceIpAddress);

        console.text += "Total addresses to scan: " + addresses.Count + "\nStarting...\nPlease wait...\n";

    }

    void startScan() {
        scanInProgress = true;
        StartCoroutine(ScanNetworkAsync());
    }

    private string GetFilePath(string fileName)
    {
        string folder = Path.Combine(Application.persistentDataPath, "Data");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        return Path.Combine(folder, fileName + ".json");
    }

    string ipaddressFile;

    public void SaveToFile(UrlDict data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFilePath(fileName), json);
        Debug.Log("Saved JSON to: " + GetFilePath(fileName));
        console.text += "Saved JSON to: " + GetFilePath(fileName) + "\n";
    }


    public UrlDict LoadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found: " + path);
            return new UrlDict(); // return empty
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<UrlDict>(json);
    }

    UrlDict cached_urls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        console.text = "";
        start = Time.time;

        cached_urls = LoadFromFile("url_cache");

        console.text += $"Cached ips: {cached_urls.ips.Count}\n";

        scanForNetworkInterfaces();

        if (ips.Count > 0)
        {
            console.text += "More than one network interface present...\nOpening dialog, please select...\n";
            GameObject panel1 = Instantiate(listPanelPrefab, pageContainerParent.transform);
            panel1.name = "chosev1";
            panel1.GetComponent<RectTransform>().localPosition = new Vector3(-10, 0, 0);

            ListPanelController lpc = panel1.GetComponent<ListPanelController>();
            lpc.setTitle("Network Interfaces");

            foreach (UnicastIPAddressInformation ip in ips)
            {
                lpc.addOption(ip.Address.ToString() + "/" + ip.IPv4Mask.ToString());
            }

            if (MarkdownManager.AnyLocalChecklists()) {

                safemode_off = 1;
                lpc.addOption("Saved Checklists");
            }

            lpc.addOption("Safe Mode");
            lpc.onOptionChosen = onChosenCallback;
        }
        else {
            console.text += "No Network Interfaces present, loading chached data...\n";

            GameObject panel1 = Instantiate(listPanelPrefab, pageContainerParent.transform);
            panel1.name = "chosev1";
            panel1.GetComponent<RectTransform>().localPosition = new Vector3(-10, 0, 0);

            ListPanelController lpc = panel1.GetComponent<ListPanelController>();
            lpc.setTitle("Network Interfaces");

            foreach (UnicastIPAddressInformation ip in ips)
            {
                lpc.addOption(ip.Address.ToString() + "/" + ip.IPv4Mask.ToString());
            }

            if (MarkdownManager.AnyLocalChecklists())
            {

                safemode_off = 1;
                lpc.addOption("Saved Checklists");
            }

            lpc.addOption("Safe Mode");
            lpc.onOptionChosen = onChosenCallback;


            //console.text += "Setting user as 'local'\n";

            //console.text += "changing scene in 4 seconds...\n";
            // currentUser = "local";

            //StartCoroutine(WaitAndLoadScene(4));


        }
    }

    void useLocalUser() {
        currentUser = "local";
        updateSingleton = true;
    }

    void useSafeModeUser()
    {
        currentUser = "__SAFE_MODE__";
        updateSingleton = true;
    }

    volatile bool scanInProgress = false;
    IEnumerator ScanNetworkAsync()
    {

        //Task[] tasks = new Task[254];
        //List<Task> tasks = new List<Task>();
        int i = 0;
        foreach (IPAddress ip in addresses)
        {
           
            StartCoroutine(TryConnect(ip.ToString()));

            if (i % 10 == 0) { yield return null; }

            i++;
        }

        //await Task.WhenAll(tasks);
        Debug.Log("Scan complete.");
        done = true;
        scanInProgress = false;
        if (!found) {
            Debug.Log("Failed in finding server...\nStarting using local cache...");


            loadCacheFailure = true;
        }


        yield return null;
    }

    bool loadCacheFailure = false;

    IEnumerator TryConnect(string ip)
    {
        if (!found)
        {


            string url = $"http://{ip}:{port}/discovery";
            string url1 = $"http://{ip}:{port}";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 100;
                request.ReadWriteTimeout = 100;
                request.Method = "GET";
                Debug.Log("req " + url);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Debug.Log($"Found: {url} - Status: {(int)response.StatusCode}");
                    found = true;
                    temp = url1;
                    getUser = true;

                }
            }
            catch (WebException)
            {
                // No response or timeout
            }
        }

        yield return null;
    }

    void scanForNetworkInterfaces()
    {
        foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (adapter.OperationalStatus != OperationalStatus.Up)
                continue;

            foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ip.IPv4Mask.ToString().StartsWith("255.255.255.")) {
                        ips.Add(ip);
                    }
                    //console.text+=("IP: " + ip.Address) +"/";
                    //console.text += ("" + ip.IPv4Mask) + "\n";
                }
            }
        }
    }

    bool lDone = false;
    bool userRetrieved = false;
    float lastDot = 0;
    bool getUser = false;
    bool updateSingleton = false;
    // Update is called once per frame
    void Update()
    {
        if (scanInProgress) {
            if (lastDot + 5 < Time.time) {
                lastDot = Time.time;
                console.text += "...\n";
            
            }
        }

       
        if (getUser)
        {
            getUser = false;
            serverURL = temp;
            console.text += "Server at " + serverURL + "\n";

            StartCoroutine(getCurrentUser());
            console.text += "Started get user coroutine... \n";

            BaseIpToUrl ltemp = new BaseIpToUrl(baseIp, serverURL, true);
            cached_urls.add(ltemp);

            SaveToFile(cached_urls, "url_cache");

        }
        //else
        //{
        // console.text += "\nCould not find server!\nplease make sure you are connected!\n";
        //console.text += "Press Left Mouse Button Green Virtual Button, or 'L' key to continue!\n";
        //}

        if (loadCacheFailure) {
            loadCacheFailure = false;
            loginAndStart();

        }


        if (currentUser != null) { 
            if(userRetrieved) {
                userRetrieved = false;
                UserResponse data = JsonUtility.FromJson<UserResponse>(jsonResponse);
                currentUser = data.user.username;
                hasCoonected = true;
                updateSingleton = true;
                Debug.Log(data.user.color);
                StyleSingleton.Instance.SetGlobalStyle(data.user.color);
                
            }

        }

        if (updateSingleton) {
            updateSingleton= false;
            loginAndStart();
        }
    }
    bool hasCoonected = false;

    private void loginAndStart() {

        console.text += "Logged in as: " + currentUser + "\n";
        console.text += serverURL;
        console.text += "\n";
        if (hasCoonected)
        {
            console.text += "starting with server...\n";
            NetworkDataSingleton.Instance.startConnection(serverURL, currentUser);


        }
        else {

            console.text += "starting without server...\n";
            NetworkDataSingleton.Instance.startNoServer(currentUser);
        }

    }

    
    private int port = 53535;

    IEnumerator getCurrentUser() {
        string url = serverURL + "/user";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        Debug.Log("req " + url);

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            Debug.Log("respc " + response.StatusCode);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd(); 
                Debug.Log($"{jsonResponse}");
                userRetrieved = true;



            }

        }
        
        yield return null;
    }

    string GetBaseIP(string ip, string netmask)
    {
        var parts = ip.Split('.');
        return $"{parts[0]}.{parts[1]}.{parts[2]}.";
    }


}
