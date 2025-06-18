using TMPro;
using UnityEngine;

public class SetUserName : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = NetworkDataSingleton.Instance.user;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
