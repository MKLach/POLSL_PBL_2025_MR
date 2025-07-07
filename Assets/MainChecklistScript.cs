using UnityEngine;

public class MainChecklistScript : MonoBehaviour
{
    public GUI_FOLLOW_Initialization guiFollow;
    public float st;

    public Transform mc;
    public Transform l;
    public Transform r;

    void Start()
    {
        st = Time.time;
        CurrentInstructionSingleton.Instance.setTransformReferences(mc, l, r);
    }

    void Update()
    {
        var ialis = InputAbstractionLayer.Instance.GetMouseButtonDown(2);
        if (ialis.queryTrue)
        {
            CurrentInstructionSingleton.Instance.setCurrentTaskAsDone(ialis);
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
