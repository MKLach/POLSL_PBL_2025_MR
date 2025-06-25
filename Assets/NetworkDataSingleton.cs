using Assets.InstructionProvider;
using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using TMPro;
using Assets.model;
using System.Linq;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine.SceneManagement;
using System.Text;

public class NetworkDataSingleton : MonoBehaviour
{
    public interface IExecutable {

        public void execute();

    }

    public class Print : IExecutable {
        public Print(string msg) {
            message = msg;
        }
        string message;
        public void execute() {
            Instance.println(message);
        }

    }

    public List<int> doneTasks = new List<int>();

    public string current_checklist_group = "__NONE__";

    public int current_checklist_id = -1;

    private string baseUrl = string.Empty;
    public string user = string.Empty;

    private IChecklistProvider checklistProvider;
    public static NetworkDataSingleton Instance { get; private set; }
   
    private Thread _thread;


    private static readonly Queue<IExecutable> _executionQueue = new Queue<IExecutable>();

    public TextMeshProUGUI console;


    public Dictionary<string, ChecklistGroup> checklists;

    public Dictionary<string, ChecklistGroup> getChecklists()
    {
        if(checklistProvider == null)
        {
            
            checklistProvider = new InMemoryChecklistProvider();

            checklistProvider.signalDownload();
            checklists = checklistProvider.GetChecklists();
            println("loaded from memory!");
        }

        return checklists;

    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
      


        DontDestroyOnLoad(gameObject);
    }


    public void println(string message) {

       

        if (console != null)
        {

            console.text += message + "\n";

        }
        
        Debug.Log(message);
        
    }

    public volatile bool isRunning = false;

    public void print12() {
        println("Hello from the netwrok thread");
    }

    public void print23() {
        println("n task!");
    }

    public void NetworkTask()
    {
        Enqueue(new Print("Hello from the netwrok thread"));
        isRunning = true;

        while (isRunning) {
            
            //getCurrentUser();
            Thread.Sleep(1000);
        }


    }

    void Start()
    {
        
    }

    
    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue()?.execute();
            }
        }

        if (checklistProvider != null) {

            if (waitingForChecklists) {
                if (checklistProvider.isReady()) {
                    checklists = checklistProvider.GetChecklists();

                    println($"Groups loaded ({checklists.Count})");
                    foreach (string key in checklists.Keys) {
                        println(key + "; " + checklists[key].instructions.Count + " checklist(s)");
                    }
                    waitingForChecklists = false;
                    println($"Advancing into main menu...");
                    StartCoroutine(intoTheMainMenu());
                }
            }
        }
    }

    IEnumerator intoTheMainMenu() {

        Enqueue(new Print("2"));
        yield return new WaitForSeconds(1);
        Enqueue(new Print("1"));
        yield return new WaitForSeconds(1);
        int toUnload =  SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(1);
        console = null;
        SceneManager.UnloadSceneAsync(toUnload);

        yield return null;
    }

    void getCurrentUser()
    {
        string jsonResponse = "";
        string url = baseUrl + "/user";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
       

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd();
                Enqueue(new Print(jsonResponse));
                
            }

        }

    }

    public void Enqueue(IExecutable action)
    {
        if (action == null)
            return;

        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    public void startConnection(string url, string user) {
        this.baseUrl= url;
        this.user= user;

        initializeChecklistProvider(new ExternalServerChecklistProvider(baseUrl));

        _thread = new Thread(NetworkTask);
        _thread.Start();


    }
    bool waitingForChecklists = false;
    public void initializeChecklistProvider(IChecklistProvider checklistProvider) {
        println("Using " + checklistProvider);
        this.checklistProvider = checklistProvider;

        StartCoroutine(this.checklistProvider.signalDownload());
        this.waitingForChecklists = true;
    }
    void OnApplicationQuit()
    {
        // Clean up the thread if it's still running
        if (_thread != null && _thread.IsAlive)
        {
            _thread.Abort(); // Not recommended in production — consider safer cancellation
        }
    }

    public void startNoServer(string currentUser)
    {
        this.user = currentUser;
        initializeChecklistProvider(user == "__SAFE_MODE__" ? new InMemoryChecklistProvider() : new SavedChecklistProvider());




    }

    public IEnumerator SendChecklistLog(string jsonPayload)
    {
        if (!string.IsNullOrEmpty(baseUrl)) { 

            string serverURL = baseUrl; // Replace with actual address
            string url = serverURL + "/checklist";

            // Create request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            // Prepare JSON payload
            // string jsonPayload = "{\"name\":\"Alex\",\"score\":42}";
            byte[] byteData = Encoding.UTF8.GetBytes(jsonPayload);

            // Write data to request stream
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteData, 0, byteData.Length);
            }

            Debug.Log("POST req to: " + url);

            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Debug.Log("Response Code: " + response.StatusCode);

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonResponse = reader.ReadToEnd();
                    Debug.Log("Response Body: " + jsonResponse);
                }
            }
        }
        yield return null;
    }
}
