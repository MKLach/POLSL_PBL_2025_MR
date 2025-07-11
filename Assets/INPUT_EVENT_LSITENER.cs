using System.Diagnostics;
using System.Threading;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class INPUT_EVENT_LSITENER : MonoBehaviour
{
    public GameObject parent_ui;
    public InputActionReference mB1;
    public GameObject leftHand;
    public GameObject rightHand;
    private FistDetector lFistDetector;
    private FistDetector rFistDetector;
    public GameObject gesture_debug;

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

        gesture_debug.GetComponentInChildren<TextMeshProUGUI>().text =
            rFistDetector.getThumb() + "\n" +
            rFistDetector.getIndex() + "\n" +
            rFistDetector.getMiddleb() + "\n" +
            rFistDetector.getRing() + "\n" +
            rFistDetector.getPinky();


        if (mB1.ToInputAction().WasPressedThisFrame() || menuGesture && !_menuPrev || lFistStateChanged || rFistStateChanged) {
            timeout = Time.time;
            UnityEngine.Debug.Log("Menu button pressed!");
	        
            GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();

            G_F.recalculate();
            G_F.toggleVisibility();
            //parent_ui.SetActive(!parent_ui.activeSelf);


            for (int i = 0; i < parent_ui.transform.childCount; i++) { 
                //parent_ui.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = !parent_ui.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled;
               // UnityEngine.Debug.Log("Child!");
            }

        }

        was_lhand_fist = isLHandFist;
        _menuPrev = menuGesture;
    }
}
