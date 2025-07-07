using System;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class ListPanelController : MonoBehaviour
{
    private GameObject textPrefab;
    private GameObject iconPrefab;

    public float ignoreInput = 0;
    public Action<GameObject, int> onOptionChosen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI a = this.transform.GetChild(currentIndex).GetComponent<TextMeshProUGUI>();
        select(a);

    }

    public string GetText(int index)
    {
        GameObject option = this.gameObject.transform.GetChild(index + 1).gameObject;

        return option.GetComponent<TextMeshProUGUI>().text;

    }

    public void setText(int index, string text)
    {
        

        GameObject option = this.gameObject.transform.GetChild(index + 1).gameObject;
        option.GetComponent<TextMeshProUGUI>().text = text;
        if (index + 1 == currentIndex)
        {
            option.GetComponent<TextMeshProUGUI>().text += " <";
        }
    }

    public void select(TextMeshProUGUI a) {
        a.fontWeight = FontWeight.Bold;
        if (!a.text.EndsWith(" <")) {
            a.text += " <";
        }
        
    }

    public void unselect(TextMeshProUGUI a)
    {
        a.fontWeight = FontWeight.Regular;
        if (a.text.EndsWith(" <")) {
            a.text = a.text.Substring(0, a.text.Length - 2);
        }
    }

    public void setTitle(string title) {
        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        inputActive = true;
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

    public void addOption(string text, string iconName)
    {
        //if (textPrefab == null)
        //{
         //   string prefabPath = "Text/defaultSystemTextPrefabWithIcon";
        //    textPrefab = Resources.Load<GameObject>(prefabPath);
        //}

        if (iconPrefab == null)
        {
            string prefabPath = "Text/defaultSystemTextPrefabWithIcon";
            iconPrefab = Resources.Load<GameObject>(prefabPath);
        }

        GameObject option = Instantiate(iconPrefab, transform);
        option.name = this.gameObject.name + "_" + (transform.childCount - 1);
        option.GetComponent<TextMeshProUGUI>().text = text;
        option.GetComponentInChildren<SetSourceImage>().setIconByString(iconName);
        option.transform.GetChild(0).GetComponent<RectTransform>().transform.localPosition += new Vector3(-6.247f, 0, 0);

        //GameObject iconObj = Instantiate(iconPrefab, option.transform);
        //iconObj.name = this.gameObject.name + "_" + (transform.childCount - 1) + "_icon";
        //iconObj.GetComponent<SetSourceImage>().setIconByString(iconName);
        //iconObj.transform.localScale = new Vector3(-0.05f, 1, 1);
        //iconObj.transform.position = new Vector3(option.transform.localPosition);

    }


    public void reactivate() {
        StartCoroutine(reacti());
    }

    public void deactivateI() { 
        inputActive = false;
    }

    IEnumerator reacti() {

        yield return new WaitForSeconds(0.5f);

        inputActive = true;

        yield return null;
    }

    

    int currentIndex = 1;
    private bool inputActive = false;

    // Update is called once per frame
    void Update()
    {
        if (inputActive)
        {
            if (InputAbstractionLayer.Instance.GetMouseButtonDown(0).queryTrue)
            {

                onOptionChosen.Invoke(this.gameObject, currentIndex - 1);
            }

            if (InputAbstractionLayer.Instance.GetMouseButtonDown(1).queryTrue)
            {

                next();

            }
            else if (InputAbstractionLayer.Instance.GetMouseButtonDown(2).queryTrue)
            {
                prev();
            }
        }
        
    }

    public void invoke(int index) {
        while (index + 1 != currentIndex) {
            next();
        }


        onOptionChosen.Invoke(this.gameObject, index);
    }

    TextMeshProUGUI getById(int idx) { 
        return this.transform.GetChild(idx).GetComponent<TextMeshProUGUI>();
    }

    void next() {
        int old = currentIndex++;

        if (currentIndex == transform.childCount)
        {
            currentIndex = transform.childCount - 1;
        }
        else {
            unselect(getById(old));
            select(getById(currentIndex));
        }
    }

    void prev() {
        int old = currentIndex--;

        if (currentIndex == 0) { currentIndex = 1; } else{

            unselect(getById(old));

            select(getById(currentIndex));
        }
    }
}
