using Assets.model;
using TMPro;
using UnityEngine;

public class CurrentTask : MonoBehaviour
{
    CurrentInstructionSingleton instructionSingleton = CurrentInstructionSingleton.Instance;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.instructionSingleton == null)
        {
            title.text = "None";
            desc.text = "NoneD";
        }
        else {
            title.text = instructionSingleton.instructionDTO.title;
            nextTask(instructionSingleton.getCurrentTask());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nextTask(TaskDTO task) {
        
        this.desc.text = (instructionSingleton.getCurrentTaskIndex()+1) +". " + task.title + "\n" + task.description;
    }

    public void allDone() {
        desc.text = "Good job!\nAll task succesfully completed!\nReturning to main in 5 seconds!";

    }

}
