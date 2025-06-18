using System.Collections;
using System.Diagnostics;
using System.Threading;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class INPUT_EVENT_LSITENER_INSTRUCTION : MonoBehaviour
{
    public GameObject parent_ui;
    public InputActionReference mB1;
    public GameObject leftHand;
    public GameObject rightHand;
    private FistDetector lFistDetector;
    private FistDetector rFistDetector;
    public CurrentTask task;

    public NotificationSystem notificationSystem;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       lFistDetector = leftHand.GetComponent<FistDetector>();
       rFistDetector = rightHand.GetComponent<FistDetector>();
        
    }

    private bool _menuPrev = false;
    private bool was_lhand_fist = false;
    private bool was_rhand_fist = false;
    private float timeout = 0;
    private float timeoutLike = 0;

    private bool was_rhand_like = false;

    // Update is called once per frame
    void Update()
    {
        bool isLHandFist = lFistDetector.isFistGesture();
        bool isRHandFist = rFistDetector.isFistGesture();


        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            UnityEngine.Debug.Log("Right Hand Grip Pressed");
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            UnityEngine.Debug.Log("Left Hand Grip Pressed");
        }

        //UnityEngine.Debug.Log(mB1.ToInputAction().triggered);
        var state = OVRPlugin.GetControllerState4((uint)OVRInput.Controller.Hands);
        bool menuGesture = (state.Buttons & (uint)OVRInput.RawButton.Start) > 0;
        
        bool lFistStateChanged = !was_lhand_fist && isLHandFist && timeout + .5 < (Time.time);
        bool rFistStateChanged = !was_rhand_fist && isRHandFist && timeout + .5 < (Time.time);
        bool keyboardM  = Input.GetKeyDown(KeyCode.M) && timeout + .5 < (Time.time);
        bool keyboardJ = Input.GetKeyDown(KeyCode.J) && timeout + .5 < (Time.time);


        if (mB1.ToInputAction().WasPressedThisFrame() || menuGesture && !_menuPrev) {
            
            UnityEngine.Debug.Log("Menu button pressed!");
	        
            GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();

            G_F.recalculate();
            G_F.toggleVisibility();
            //parent_ui.SetActive(!parent_ui.activeSelf);
        }

        if (rFistStateChanged || lFistStateChanged || keyboardM) {
            timeout = Time.time;
            notificationSystem.notify("Task Completed", "" + (CurrentInstructionSingleton.Instance.getCurrentTaskIndex()+1) + "/"+ CurrentInstructionSingleton.Instance.instructionDTO.tasks.Count, 2);
            CurrentInstructionSingleton.Instance.setCurrentTaskAsDone();

            if (CurrentInstructionSingleton.Instance.hasBeenCompleted())
            {
                task.allDone();
                StartCoroutine(WaitAndLoadScene(5f));
            }
            else { 
                task.nextTask(CurrentInstructionSingleton.Instance.getCurrentTask());
            }


        }

        if (keyboardJ) {
            timeout = Time.time;
            notificationSystem.notify("Previous Task", "Going back to previous task!", 2);
            task.prevTask();
        }

        was_rhand_fist = isRHandFist;
        was_lhand_fist = isLHandFist;
        _menuPrev = menuGesture;
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
    }
}
