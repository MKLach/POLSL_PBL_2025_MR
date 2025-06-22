using Assets.model;
using Meta.XR.ImmersiveDebugger.UserInterface;
using System;
using TMPro;
using UnityEngine;
using static NetworkDataSingleton;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;

[Serializable]
public class Task { 

}

[Serializable]
public class Checklist { 

}

[Serializable]
public class TransformData
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;

    public TransformData(Transform t) {
        this.position = t.position;
        this.rotation = t.rotation;
        scale = t.localScale;
    }

    internal TransformData() { 
    
    }

    public TransformData(TransformData other)
    {
        position = other.position;
        rotation = other.rotation;
        scale = other.scale;
    }
}


[Serializable]
public class ChMgr {
    ChecklistController chcr;

    public ChMgr()
    {
        chcr = new ChecklistController();

    }


}

[Serializable]
public class ChecklistController {
    [NonSerialized]
    public List<ChecklistController> previous;

    public int taskIndex;

    public int prevIndex;

    public int inputMode;
    public TransformData mainCameraPos;
    public TransformData leftHandPos;
    public TransformData righttHandPos;

    public float currentTimeSinceAppStart;
    public float startedAt;

    public string startedAtDateTime;
    public string currentDateTime;

    public ChecklistController(int taskIndex) {
        previous = new List<ChecklistController>();
        taskIndex = 0;
        prevIndex = -1;
        inputMode = 0;
        mainCameraPos = new TransformData();
        leftHandPos = new TransformData();
        righttHandPos = new TransformData();

        previous.Add(new ChecklistController(this));

    }

    public ChecklistController() { }

    public ChecklistController(ChecklistController cc) {
        previous = new List<ChecklistController>();
        foreach (var item in cc.previous)
        {
            Debug.Log(item.Equals(cc));
            if (item.Equals( cc )) {
                break;
            }
            previous.Add(item);
        }
        //previous.Add(new ChecklistController(this));

        taskIndex = cc.taskIndex;
        inputMode = cc.inputMode;
        prevIndex = cc.prevIndex;


        mainCameraPos = new TransformData(cc.mainCameraPos);
        leftHandPos = new TransformData(cc.leftHandPos);
        righttHandPos = new TransformData(cc.righttHandPos);

        currentTimeSinceAppStart = cc.currentTimeSinceAppStart;
        startedAt = cc.startedAt;

        startedAtDateTime = cc.startedAtDateTime;
        currentDateTime = cc.currentDateTime;
        updateTime();
    }

    void updateTime() {
        this.startedAt = Time.time;
        this.currentTimeSinceAppStart = Time.time;

        this.startedAtDateTime = DateTime.UtcNow.ToString();
        this.currentDateTime = DateTime.UtcNow.ToString();
    }

    public void persist(Transform m, Transform l, Transform r, int im, int nextTask) {
        this.mainCameraPos = new TransformData(m);
        this.leftHandPos = new TransformData(l);
        this.righttHandPos = new TransformData(r);
        this.inputMode = im;
        int old = taskIndex;

        this.previous.Add(new ChecklistController(this));

        this.taskIndex = nextTask;
        this.prevIndex = old;

    }

    public ChecklistController restart() {
        this.previous.RemoveAll((p) => {
            return p.taskIndex > 0;  
        });
      
        updateTime();
        return this.previous.Last();
    }

    public ChecklistController current()
    {
        return this;
    }

    public ChecklistController restore(int indexToReadFrom) {
        int old = this.taskIndex;
        this.previous.RemoveAll((p) => {
            return p.taskIndex > indexToReadFrom;
        });
        this.previous.Last().prevIndex = old;
        return this.previous.Last();
    }

    public List<ChecklistController> all()
    {
        return this.previous;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        ChecklistController other = (ChecklistController)obj;

        return other.taskIndex == this.taskIndex;
    }
}

[Serializable]
public class Log {
    public List<ChecklistController> logs;
}

public class CurrentInstructionSingleton : MonoBehaviour
{
    public static CurrentInstructionSingleton Instance;
    public InstructionDTO instructionDTO;
    //int currentTaskIndex = 0;
    //int latestDoneIndex = -1;
    private ListPanel currentTaskPanel;
    float startTime = 0;
    float totalTime = 0;

    Transform mainCameraTransform;
    Transform leftHandTransform;
    Transform rightHandTransform;

    public void setTransformReferences(Transform mainCameraTransform, Transform leftHandTransform, Transform rightHandTransform) { 
        this.mainCameraTransform = mainCameraTransform;
        this.leftHandTransform = leftHandTransform;
        this.rightHandTransform = rightHandTransform;
    }

    ChecklistController checklistController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void setListPanel(ListPanel listPanel) {
        currentTaskPanel = listPanel;
        startTime = Time.time;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
            checklistController = new ChecklistController(0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool done = false;

    public void setInstruction(InstructionDTO ins)
    {
        done = false;
        this.instructionDTO= ins;
        checklistController = checklistController.restart();
        //currentTaskIndex = 0;
    }

    public TaskDTO getCurrentTask() {
        if (checklistController.current().taskIndex >= instructionDTO.tasks.Count) {
            return null;
        }
        return instructionDTO.tasks[checklistController.current().taskIndex];
    }

    public int getCurrentTaskIndex(){ 
        return checklistController.current().taskIndex;
    }

    public bool hasBeenCompleted()
    {
        return done;
    }

    public bool hasBeenCompleted(int taskindex)
    {
        return checklistController.current().taskIndex > taskindex;
    }

    public string setAsDone(string text) {
        string mod = "X " + text.Substring(2);
        return mod;
    }

    public string setAsClear(string text)
    {
        string mod = "  " + text.Substring(2);
        return mod;
    }
    public string setAsToDo(string text)
    {
        string mod = "> " + text.Substring(2);
        return mod;
    }

    public void recalcHistory()
    {
        for (int i = 0; i < instructionDTO.tasks.Count; i++)
        {
            if (i < checklistController.current().taskIndex)
            {
                currentTaskPanel.setText(i, setAsDone(currentTaskPanel.GetText(i)));
            }
            else if (i == checklistController.current().taskIndex) {
                currentTaskPanel.setText(i, setAsToDo(currentTaskPanel.GetText(i)));

            }

            else
            {
                currentTaskPanel.setText(i, setAsClear(currentTaskPanel.GetText(i)));
            }
        }
    }

    public void setCurrentTaskAsDone()
    {
        if(done) { return; }
        //int lod = latestDoneIndex;
        //int oldIndex = currentTaskIndex;
        int currentTaskIndex = checklistController.current().taskIndex;

        int nextIndex = checklistController.current().taskIndex + 1;

        if (nextIndex >= instructionDTO.tasks.Count) { nextIndex -= 1; } else {
            
            checklistController.persist(mainCameraTransform, leftHandTransform, rightHandTransform, 0x0, nextIndex);

        }

       recalcHistory();

        currentTaskIndex++;

        if (currentTaskIndex >= instructionDTO.tasks.Count)
        {
            done = true;
            currentTaskIndex -= 1;
            currentTaskPanel.setText(currentTaskIndex, setAsDone(currentTaskPanel.GetText(currentTaskIndex)));
            totalTime = Time.time - startTime;
            currentTaskOutput.text = $"All done\nTime: {(int)totalTime}s\nReturning to menu";

            Log log = new Log();
            log.logs = checklistController.all();

            string payload = JsonUtility.ToJson(log);

            StartCoroutine(NetworkDataSingleton.Instance.SendChecklistLog(payload));


            StartCoroutine(intoTheMainMenu());
        }
        else {
            //if (oldIndex <= lod) {
            //currentTaskPanel.setText(currentTaskIndex, setAsToDo(currentTaskPanel.GetText(currentTaskIndex)));
            //}
            
            updateTextOutput();

        }

    }

    IEnumerator intoTheMainMenu()
    {

        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1);
        int toUnload = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(1);
        SceneManager.UnloadSceneAsync(toUnload);

        yield return null;
    }

    public void goPreviousTask()
    {
        //currentTaskIndex--;
    }

    private TextMeshProUGUI currentTaskOutput;
    public void setCurrentTaskText(TextMeshProUGUI textMeshProUGUI)
    {
        currentTaskOutput = textMeshProUGUI;

        updateTextOutput();


    }

    public void updateTextOutput() {
        var t = getCurrentTask();

        currentTaskOutput.text = t.shortTitle;
        currentTaskOutput.text += "\n";
        currentTaskOutput.text += t.description;

    }

    public void visitPrevious()
    {
        if (checklistController.current().taskIndex <= 0) {
            return;
        }
        //goPreviousTask();
        //updateTextOutput();
        Debug.Log("restoring");
        checklistController=checklistController.restore(checklistController.current().taskIndex - 1);

        recalcHistory();

    }

   
}
