using System;
using System.Reflection;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class ListPanel : MonoBehaviour
{
    private GameObject textPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public string GetText(int index) {
        GameObject option = this.gameObject.transform.GetChild(index+1).gameObject;
        
        return option.GetComponent<TextMeshProUGUI>().text;

    }

    public void setText(int index, string text)
    {
        GameObject option = this.gameObject.transform.GetChild(index + 1).gameObject;
        option.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void setTitle(string title) {
        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
    }

    public void addOption(string text) {
        if (textPrefab == null) {
            string prefabPath = "Text/defaultSystemTextPrefab";
            textPrefab = Resources.Load<GameObject>(prefabPath);
        }

        GameObject option = Instantiate(textPrefab, transform);
        option.name = this.gameObject.name + "_" + (transform.childCount - 1);
        option.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void clear() { 
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
       

    }
}
