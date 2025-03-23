using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class INPUT_EVENT_LSITENER : MonoBehaviour
{
    public GameObject parent_ui;
    public InputActionReference mB1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }


    private bool _menuPrev = false;
    // Update is called once per frame
    void Update()
    {
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

        if (mB1.ToInputAction().WasPressedThisFrame() || menuGesture && !_menuPrev) {
            UnityEngine.Debug.Log("Menu button pressed!");
	        
            GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();

            G_F.recalculate();
            parent_ui.SetActive(!parent_ui.activeSelf);


            for (int i = 0; i < parent_ui.transform.childCount; i++) { 
                //parent_ui.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = !parent_ui.transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled;
               // UnityEngine.Debug.Log("Child!");
            }

        }

       
        _menuPrev = menuGesture;
    }
}
