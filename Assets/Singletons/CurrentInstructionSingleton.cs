using Assets.model;
using Meta.XR.ImmersiveDebugger.UserInterface;
using System;
using TMPro;
using UnityEngine;
using static NetworkDataSingleton;
using UnityEngine.SceneManagement;
using System.Collections;

public class CurrentInstructionSingleton : MonoBehaviour
{
    public static CurrentInstructionSingleton Instance;
    public InstructionDTO instructionDTO;
    int currentTaskIndex = 0;
    int latestDoneIndex = -1;
    private ListPanel currentTaskPanel;
    float startTime = 0;
    float totalTime = 0;
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
        
        currentTaskIndex = 0;
    }

    public TaskDTO getCurrentTask() {
        if (currentTaskIndex >= instructionDTO.tasks.Count) {
            return null;
        }
        return instructionDTO.tasks[currentTaskIndex];
    }

    public int getCurrentTaskIndex(){ 
        return currentTaskIndex;
    }

    public bool hasBeenCompleted()
    {
        return done;
    }

    public bool hasBeenCompleted(int taskindex)
    {
        return currentTaskIndex > taskindex;
    }

    public string setAsDone(string text) {
        string mod = "X " + text.Substring(2);
        return mod;
    }

    public string setAsToDo(string text)
    {
        string mod = "> " + text.Substring(2);
        return mod;
    }


    public void setCurrentTaskAsDone()
    {
        if(done) { return; }
        int lod = latestDoneIndex;
        int oldIndex = currentTaskIndex;
        
        currentTaskPanel.setText(currentTaskIndex, setAsDone(currentTaskPanel.GetText(currentTaskIndex)));
        latestDoneIndex = currentTaskIndex;
        

        currentTaskIndex++;

        if (currentTaskIndex >= instructionDTO.tasks.Count)
        {
            done = true;
            currentTaskIndex -= 1;
            currentTaskPanel.setText(currentTaskIndex, setAsDone(currentTaskPanel.GetText(currentTaskIndex)));
            totalTime = Time.time - startTime;
            currentTaskOutput.text = $"All done\nTime: {(int)totalTime}s\nReturning to menu";
            StartCoroutine(intoTheMainMenu());
        }
        else {
            //if (oldIndex <= lod) {
            currentTaskPanel.setText(currentTaskIndex, setAsToDo(currentTaskPanel.GetText(currentTaskIndex)));
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
        currentTaskIndex--;
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
        if (currentTaskIndex <= 0) {
            return;
        }
        goPreviousTask();
        updateTextOutput();




    }
}
