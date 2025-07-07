using Assets.model;
using Mono.Reflection;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditorInternal.ReorderableList;

public class SetSourceImage : MonoBehaviour
{
   
    public Sprite defaut;
    public Sprite warning; 
    public Sprite info;
    public Sprite question;
    public Sprite speed;
    public Sprite none;

    void Start()
    {
        /*defaut = Resources.Load<Sprite>("icons/default");
        warning = Resources.Load<Sprite>("icons/warning");
        info = Resources.Load<Sprite>("icons/info");
        question = Resources.Load<Sprite>("icons/question_mark");
        speed = Resources.Load<Sprite>("icons/speed");*/

        //GetComponent<Image>().sprite = info;
        GetComponent<Image>().color = StyleSingleton.Instance.textColor;
        
    }

    public void setIconByInstruction(InstructionDTO instruction) { 
        

       


    }

    
    void Update()
    {
        
    }

    internal void setIconByString(string iconName)
    {
        switch (iconName)
        {
            default:
                GetComponent<Image>().sprite = defaut;
                return;
            case "warning":
                GetComponent<Image>().sprite = warning;
                return;
            case "info":
                GetComponent<Image>().sprite = info;
                return;
            case "question_mark":
                GetComponent<Image>().sprite = question;
                return;
            case "speed":
                GetComponent<Image>().sprite = speed;
                return;
            case "none":
                GetComponent<Image>().sprite = none;
                return;


        }


    }
}
