using Assets.model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static NetworkDataSingleton;
using UnityEngine.SceneManagement;
using Assets.markdown;

public class MainMenuMainScript : MonoBehaviour
{
    
    public GameObject mainPage;

    GameObject listPrefab;
    GameObject readOnlyListPrefab;

    public GUI_FOLLOW_Initialization guiFollow;

    Stack<ListPanelController> listPanels = new Stack<ListPanelController>();

    ListPanelController root;

    void Start()
    {
        if (listPrefab == null)
        {
            string prefabPath = "Text/listPanel";
            listPrefab = Resources.Load<GameObject>(prefabPath);
        }

        if (readOnlyListPrefab == null)
        {
            string prefabPath = "Text/readOnlyListPanel";
            readOnlyListPrefab = Resources.Load<GameObject>(prefabPath);
        }

        GameObject options = Instantiate(listPrefab, mainPage.transform);

        options.name = "mainMenu";
        ListPanelController lpc = options.GetComponent<ListPanelController>();
        lpc.setTitle("Main Menu");

        foreach (var key in NetworkDataSingleton.Instance.getChecklists().Keys) {
            lpc.addOption(key + " - checklists");
        }

        lpc.addOption("Disable Gestrues");
        lpc.addOption("Purge checklists");
        lpc.addOption("Hide Menu");
        lpc.GetComponent<RectTransform>().localPosition = new Vector3(-2.4f, -1.75f, 0);

        lpc.onOptionChosen = onMainMenuSelect;
        root = lpc;
        addPanel(lpc);
        
        if (NetworkDataSingleton.Instance.current_checklist_group != "__NONE__") {
            int i = 0;
            foreach (string key in NetworkDataSingleton.Instance.getChecklists().Keys) {

                if (key == NetworkDataSingleton.Instance.current_checklist_group) {
                    lpc.invoke(i);
                    break;
                }
                i++;
            }
        }

    }

    public void addPanel(ListPanelController lpc) {
        ListPanelController current = null;
        if (listPanels.TryPeek(out current)) {
            current.deactivateI();
        }

        listPanels.Push(lpc);
        Debug.Log(listPanels.Count);

    }

    public void goBack() {
        listPanels.Pop();
         ListPanelController current = null;
        if (listPanels.TryPeek(out current)) {
            current.reactivate();
        }
    }

    bool hidden = false;

    public void togglePoseRecognition() { 
        
    }
    public void onMainMenuSelect(GameObject listPanel, int index) {

        if (NetworkDataSingleton.Instance.getChecklists().Count + 1 == index)
        {
            if (root.GetText(index) != "Purged! (restart req.)") {
                MarkdownManager.purgeAllChecklists();
                root.setText(index, "Purged! (restart req.)");
            }
            
        } 
        else if (NetworkDataSingleton.Instance.getChecklists().Count + 2 == index)
        {
            hide();
        }
        else if (NetworkDataSingleton.Instance.getChecklists().Count == index)
        {
            if (InputAbstractionLayer.Instance.poseRecognitionEnabled)
            {
                InputAbstractionLayer.Instance.poseRecognitionEnabled = false;
                root.setText(index, "Enable Gestures");

            }
            else {
                InputAbstractionLayer.Instance.poseRecognitionEnabled = true;
                root.setText(index, "Disable Gestures");
            }

           
        }
        else {
            GameObject checklists = Instantiate(listPrefab, mainPage.transform);
            checklists.name = "checklistGroup";
            ListPanelController lpc = checklists.GetComponent<ListPanelController>();
            
            string name = NetworkDataSingleton.Instance.getChecklists().Keys.ToArray()[index];
            NetworkDataSingleton.Instance.current_checklist_group = name;
            groups = NetworkDataSingleton.Instance.getChecklists()[name];
            lpc.setTitle(name);
            lpc.addOption("Back");

            foreach (var key in groups.instructions.Values)
            {
                lpc.addOption(key.shortTitle);
            }
            lpc.onOptionChosen = onChecklistSelected;
            lpc.GetComponent<RectTransform>().localPosition = new Vector3(-10, -1.75f - 0.0f * (index+1), 0);

            addPanel(lpc);

        }

        //Destroy(listPanel);
    }
    ChecklistGroup groups;
    InstructionDTO selected;

    GameObject taskListV1;
    public void onChecklistSelected(GameObject listPanel, int index) {
        // back
        if (index == 0)
        {
            groups = null;
            goBack();
            Destroy(listPanel);
        }
        else {
            GameObject taskInChecklsit = Instantiate(listPrefab, mainPage.transform);
            taskInChecklsit.name = "checklist";
            selected = groups.instructions.Values.ToArray()[index-1];

            ListPanelController lpc = taskInChecklsit.GetComponent<ListPanelController>();
            lpc.GetComponent<RectTransform>().localPosition = new Vector3(-14, -1.75f - 0.0f * (index + 1), 0);


            lpc.setTitle("Proceed?");
            lpc.addOption("Yes");
            lpc.addOption("NO");
            lpc.onOptionChosen = onStartChecklist;

            addPanel(lpc);

            taskListV1 = Instantiate(readOnlyListPrefab, mainPage.transform);
            ListPanel lpcRO = taskListV1.GetComponent<ListPanel>();
            lpcRO.GetComponent<RectTransform>().localPosition = new Vector3(-14, -3f, 0);
            lpcRO.setTitle(selected.shortTitle);
            foreach (var task in selected.tasks) {

                lpcRO.addOption(task.title);

            }

        }

    }
   

    public void onStartChecklist(GameObject listPanel, int index)
    {
        Destroy(taskListV1);
        taskListV1 = null;

        if (index == 0)
        {
            CurrentInstructionSingleton.Instance.setInstruction(selected);

            StartCoroutine(intoTheChecklistScreen());
        }
        else {

            goBack();
            Destroy(listPanel);
        }
        
       

    }

    IEnumerator intoTheChecklistScreen()
    {

       
        int toUnload = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(2);
        SceneManager.UnloadSceneAsync(toUnload);

        yield return null;
    }


    void hide()
    {
        guiFollow.changeVisibility(false);

        foreach (var panel in listPanels)
        {
            panel.deactivateI();
        }

        hidden = true;
    }
    void unhide() {
        guiFollow.changeVisibility(true);
        hidden = false;

        ListPanelController current = null;
        if (listPanels.TryPeek(out current))
        {
            current.reactivate();
        }

    }

    
    void Update()
    {
        
        if (hidden) {
            
            if (InputAbstractionLayer.Instance.GetMouseButtonDown(0).queryTrue)
            {
               
                unhide();

            }
        }
    }
}
