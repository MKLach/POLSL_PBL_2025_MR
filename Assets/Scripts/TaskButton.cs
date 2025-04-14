using Assets.model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    public void Initialize(int number, string text, InteractiveJournal controller, TaskDTO data)
    {
        if (buttonText != null)
        {
            buttonText.text = $"{number}. {text}";
        }

       // GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => controller.ShowTaskDetail(null));
    }

    public void InitializeIns(int number, string text, InteractiveJournal controller, InstructionDTO data)
    {
        if (buttonText != null)
        {
            buttonText.text = $"{number}. {text}";
        }

        //GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => controller.ShowInstructionDetail(data));
    }

    public void OnKeyPressed()
    {
        GetComponent<Button>().onClick.Invoke();
    }

    private void OnDestroy()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}