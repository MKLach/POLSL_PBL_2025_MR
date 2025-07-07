using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.model;
using TMPro;
using Oculus.Platform.Models;
using Meta.XR.ImmersiveDebugger.UserInterface;

public class InteractiveJournal : MonoBehaviour
{
    [Header("UI References")]
    public Canvas journalCanvas;
    public TextMeshProUGUI listPageTitleText;
    public TextMeshProUGUI detailPageTitleText;
    public Transform taskButtonsContainer;
    public TextMeshProUGUI detailContentText;
    public GameObject listPagePanel;
    public GameObject detailPagePanel;
    public TaskButton taskButtonPrefab;
    public Button returnButton;
    public TextMeshProUGUI returnButtonText;

    public GameObject CURRENT_INSTRUCTION_DATA;


    public Button startButton;
    public TextMeshProUGUI startButtonText;

    [Header("Navigation")]
    public KeyCode returnKey = KeyCode.Alpha0;
    public KeyCode backKey = KeyCode.Escape;

    private InstructionsRepository repository;
    private List<TaskButton> taskButtons = new List<TaskButton>();
    private Stack<JournalInstructionPage> pageStack;

    private class JournalInstructionPage
    {
        public string title;
        public List<InstructionDTO> tasks;
        public bool isMainPage;

        public JournalInstructionPage(string title, List<InstructionDTO> tasks, bool isMainPage)
        {
            this.title = title;
            this.tasks = tasks;
            this.isMainPage = isMainPage;
        }
    }

    private class JournalPageOld
    {
        public string title;
        public List<TaskDTO> tasks;
        public bool isMainPage;

        public JournalPageOld(string title, List<TaskDTO> tasks, bool isMainPage)
        {
            this.title = title;
            this.tasks = tasks;
            this.isMainPage = isMainPage;
        }
    }

    void Start()
    {
        pageStack = new Stack<JournalInstructionPage>();
        repository = gameObject.AddComponent<InstructionsRepository>();
        if (repository == null)
        {
            Debug.LogError("InstructionsRepository not found in scene!");
            return;
        }
        repository.initv1();

        returnButton.onClick.AddListener(OnReturnPressed);
        startButton.onClick.AddListener(OnStartPressed);

        UpdateReturnButton(false);
        UpdateStartButton(false);

        Debug.LogError(repository.get().Count);
        if (repository.get().Count > 0)
        {
            ShowTaskList("Instructions",
                        repository.get(),
                        true);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(returnKey) && returnButton.gameObject.activeSelf)
        {
            OnReturnPressed();
        }
        else if (Input.GetKeyDown(backKey))
        {
            OnBackPressed();
        }

        if (listPagePanel.activeSelf)
        {
            for (int i = 0; i < taskButtons.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    taskButtons[i].OnKeyPressed();
                    break;
                }
            }
        }
    }

    void CreateTaskButtons(List<TaskDTO> tasks)
    {
        ClearAllTaskButtons();

        for (int i = 0; i < tasks.Count; i++)
        {
            TaskButton taskButton = Instantiate(taskButtonPrefab, taskButtonsContainer);
            taskButton.Initialize(i + 1, tasks[i].title, this, tasks[i]);



            taskButton.GetComponent<BoxCollider>().size = new Vector3(taskButton.GetComponent<RectTransform>().rect.width, taskButton.GetComponent<RectTransform>().rect.height, 0.2f);
            taskButtons.Add(taskButton);
        }
    }

    void CreateInsButtons(List<InstructionDTO> tasks)
    {
        ClearAllTaskButtons();

        for (int i = 0; i < tasks.Count; i++)
        {
            TaskButton taskButton = Instantiate(taskButtonPrefab, taskButtonsContainer);
            taskButton.InitializeIns(i + 1, tasks[i].iconName, this, tasks[i]);



            taskButton.GetComponent<BoxCollider>().size = new Vector3(taskButton.GetComponent<RectTransform>().rect.width, taskButton.GetComponent<RectTransform>().rect.height, 0.2f);
            taskButtons.Add(taskButton);
        }
    }

    void ClearAllTaskButtons()
    {
        // First disable the container to hide all buttons immediately
        taskButtonsContainer.gameObject.SetActive(false);

        // Then destroy all button GameObjects
        foreach (var button in taskButtons)
        {
            if (button != null && button.gameObject != null)
            {
                Destroy(button.gameObject);
            }
        }
        taskButtons.Clear();

        // Re-enable the container if we're on list page
        if (listPagePanel.activeSelf)
        {
            taskButtonsContainer.gameObject.SetActive(true);
        }
    }

    public void ShowTaskList(string title, List<InstructionDTO> tasks, bool isMainPage = false)
    {
        pageStack.Push(new JournalInstructionPage(title, tasks, isMainPage));
        UpdateCurrentPage();
    }

    public void ShowTaskDetail(InstructionDTO task)
    {
        /*  if (task.subTasks.Count > 0)
          {
              ShowTaskList(task.title, task.subTasks);
          }
          else
          {
              detailPageTitleText.text = task.title;
              detailContentText.text = $"{task.description}";

              // Properly hide list page and show detail page
              listPagePanel.SetActive(false);
              detailPagePanel.SetActive(true);
              taskButtonsContainer.gameObject.SetActive(false); // Ensure buttons are hidden
              UpdateReturnButton(true);
          }*/
    }

    public void ShowInstructionDetail(InstructionDTO ins)
    {
        Debug.Log(ins);
        Debug.Log(CurrentInstructionSingleton.Instance);

        CurrentInstructionSingleton.Instance.setInstruction(ins);
        detailPageTitleText.text = ins.iconName;

        string txt = "";
        int i = 0;
        foreach(TaskDTO task in ins.tasks)
        {
            txt += (i+1) + ". " + task.shortTitle + "\n";
            i++;
        }

        detailContentText.text = $"{txt}";

        // Properly hide list page and show detail page
        listPagePanel.SetActive(false);
        detailPagePanel.SetActive(true);
        taskButtonsContainer.gameObject.SetActive(false); // Ensure buttons are hidden
        UpdateReturnButton(true);
        UpdateStartButton(true);
        
    }

    void UpdateCurrentPage()
    {
        if (pageStack.Count == 0) return;

        var currentPage = pageStack.Peek();

        listPageTitleText.text = currentPage.title;
        CreateInsButtons(this.repository.get());

        listPagePanel.SetActive(true);
        detailPagePanel.SetActive(false);
        taskButtonsContainer.gameObject.SetActive(true); // Show buttons container
        UpdateReturnButton(!currentPage.isMainPage);
        UpdateStartButton(!currentPage.isMainPage);
    }

    void UpdateReturnButton(bool show)
    {
        returnButton.gameObject.SetActive(show);
        returnButtonText.text = $"Return";
    }

    void UpdateStartButton(bool show)
    {
        startButton.gameObject.SetActive(show);
        startButtonText.text = $"Start";
    }


    public void OnReturnPressed()
    {
        if (detailPagePanel.activeSelf)
        {
            UpdateCurrentPage();
        }
        else if (pageStack.Count > 1)
        {
            pageStack.Pop();
            UpdateCurrentPage();
        }
    }

    public void OnStartPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
    }

    public void OnBackPressed()
    {
        if (pageStack.Count > 1)
        {
            pageStack.Pop();
            UpdateCurrentPage();
        }
    }
}