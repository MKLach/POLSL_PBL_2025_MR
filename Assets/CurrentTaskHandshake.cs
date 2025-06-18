using TMPro;
using UnityEngine;

public class CurrentTaskHandshake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentInstructionSingleton.Instance.setCurrentTaskText(this.GetComponent<TextMeshProUGUI>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
