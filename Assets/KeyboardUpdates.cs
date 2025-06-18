using TMPro;
using UnityEngine;

public class KeyboardUpdates : MonoBehaviour
{
    public TextMeshProUGUI m_TextMeshProUGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // m_TextMeshProUGUI.text += "\nHelloFromKeyboard system!!";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Some key was pressed!");
           // m_TextMeshProUGUI.text += "\nAnyKeyPressed!";
        }
    }
}
