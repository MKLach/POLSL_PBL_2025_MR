using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputAbstractionLayer : MonoBehaviour {

    public static InputAbstractionLayer Instance;

    public struct InputState {
        public bool queryTrue;
        public int abstractSource;
    }

    private GameObject posesManager;

    Dictionary<int, PoseInput> poses = new Dictionary<int, PoseInput>();
    PoseInput[] posesList;

    public PoseInput enablePoseInputPose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //posesList = posesManager.GetComponents<PoseInput>();

        //foreach (PoseInput poseInput in posesList)
        //{
        //    poses.Add(poseInput.mouseButtonBinding, poseInput);
        //}


    }

    public void setPoseEnabler(PoseInput poseInput)
    {
        this.enablePoseInputPose = poseInput;

    }

    public void setPosesManager(GameObject pm) {
        this.posesManager = pm;

        poses.Clear();
        posesList = posesManager.GetComponents<PoseInput>();

        foreach (PoseInput poseInput in posesList)
        {
            poses.Add(poseInput.mouseButtonBinding, poseInput);
        }
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

    // Update is called once per frame
    void Update()
    {
        foreach (PoseInput input in posesList)
        {
            input.UpdateLogic();
        }
        enablePoseInputPose.UpdateLogic();
    }
    public bool poseRecognitionEnabled = true;
    public InputState GetMouseButtonDown(int button) {
        InputState _is = new InputState();
        _is.queryTrue = false;
        if (Input.GetMouseButtonDown(button)) {
            _is.abstractSource = 0x0;
            _is.queryTrue = true;
            return _is;
        }

        if (!poseRecognitionEnabled) { return _is; }

        if (poses[button].IsDownThisFrame) {

            if (button == 0)
            {
                _is.abstractSource = 0x1;
                _is.queryTrue = true && poses[button].checkAgainsLastUsed();
                return _is;

            }
            else {
                _is.abstractSource = 0x1;
                _is.queryTrue = true && enablePoseInputPose.selected && poses[button].checkAgainsLastUsed();
                return _is;
            }
            

            
        }

        return _is;


    }

}
