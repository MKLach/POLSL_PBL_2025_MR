using Assets.model;
using UnityEngine;

public class CurrentInstructionSingleton : MonoBehaviour
{
    public static CurrentInstructionSingleton Instance;
    public InstructionDTO instructionDTO;
    int currentTaskIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void setCurrentTaskAsDone()
    {
        if(done) { return; }

        currentTaskIndex++;
        if (currentTaskIndex >= instructionDTO.tasks.Count)
        {
            done = true;
            currentTaskIndex -= 1;
        }
       
    }


}
