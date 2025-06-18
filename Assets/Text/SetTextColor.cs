using TMPro;
using UnityEngine;

public class SetTextColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().color = StyleSingleton.Instance.textColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
