using UnityEngine;

public class MainChecklistScript : MonoBehaviour
{
    public GUI_FOLLOW_Initialization guiFollow;
    public float st;
    void Start()
    {
        st = Time.time;
    }

    void Update()
    {

        if (InputAbstractionLayer.Instance.GetMouseButtonDown(2).queryTrue)
        {
            CurrentInstructionSingleton.Instance.setCurrentTaskAsDone();
        }

        if (InputAbstractionLayer.Instance.GetMouseButtonDown(1).queryTrue)
        {
            CurrentInstructionSingleton.Instance.visitPrevious();
        }

        if (InputAbstractionLayer.Instance.GetMouseButtonDown(0).queryTrue)
        {
            if(st + 3 < Time.time)
            {
                guiFollow.toggleVisibility();
            }
            
        }
    }
}
