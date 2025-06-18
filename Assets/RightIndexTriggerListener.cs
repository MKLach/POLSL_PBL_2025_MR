using System;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class RightIndexTriggerListener : MonoBehaviour
{
    public GameObject insList;
    public Button startButtion;
    public Button returnButtion;
    bool detailsStage = false;
    public OVRHand rightHand;


    GameObject currentlySelected;
    public int currentlySelected_index = 0;
    float lastEvent = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        while (insList.transform.childCount == 0)
        {
            Debug.Log("WAIT");
        }
        onGuiSceneChanged(5);
    }


    public void onGuiSceneChanged(int insListCount) {

        select(0);
       
        Debug.LogError("CHIDLREN FOUND");

    }

    public void unselect() {
        if(currentlySelected!= null)
        {
            string temp = currentlySelected.GetComponentInChildren<TextMeshProUGUI>().text;
            if (temp.EndsWith(" <"))
            {
                temp = temp.Substring(0, temp.Length - 2); // Remove last 2 characters
            }
            currentlySelected.GetComponentInChildren<TextMeshProUGUI>().text = temp;
            currentlySelected.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }

        
    }

    public void select(int index) {
        unselect();

        
        currentlySelected = insList.transform.GetChild(index).gameObject;
        currentlySelected_index = index;
        currentlySelected.GetComponentInChildren<TextMeshProUGUI>().text += " <";
        currentlySelected.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }

    public bool selectNext() {

        if (currentlySelected_index == insList.transform.childCount - 1) { return false; }

        select(currentlySelected_index + 1);

        return true;
    }

    public bool selectPrev()
    {

        if (currentlySelected_index == 0) { return false; }

        select(currentlySelected_index - 1);

        return true;
    }

    float mEvent = 0;
    float jEvent = 0;
    float lEvent = 0;
    float kEvent = 0;

    bool truig = false;

    // Update is called once per frame
    void Update()
    {
        float scrollInput = -Input.GetAxis("Mouse ScrollWheel");

        if (currentlySelected == null) {
            Debug.Log("RE CONFIOGURING");
            onGuiSceneChanged(5);

        }

        if(!truig && Time.time > 5) {
            truig = true;
            triggerButton();
           
        }
            
        if (Input.GetKeyDown(KeyCode.M) || scrollInput > 0f) {
           
            if (mEvent + 0.6 > Time.time) {
               

            } else {
                mEvent = Time.time;
                lastEvent = Time.time;
                
                if (!detailsStage) {
                    
                    selectNext();
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.J) || scrollInput < 0f)
        {
            if (jEvent + 0.6 > Time.time)
            {

            }
            else
            {
                jEvent = Time.time;
                lastEvent = Time.time;
                if (!detailsStage)
                {
                    selectPrev();

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L) || Input.GetMouseButtonDown(0))
        {
            if (lEvent + 0.6 > Time.time)
            {

            }
            else
            {
                lEvent = Time.time;
                lastEvent = Time.time;
                
                if (!detailsStage)
                {
                    Debug.Log("AAAAA");
                    triggerButton();
                }
                else {
                    startButtion.onClick.Invoke();
                }
               

                //selectPrev();
            }
        }

        if (Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(1))
        {
            if (kEvent + 0.6 > Time.time)
            {

            }
            else
            {
                kEvent = Time.time;
                lastEvent = Time.time;
                if (detailsStage) {
                    detailsStage = false;
                    returnButtion.onClick.Invoke();

                }
                //selectPrev();
            }
        }
    }

    public void triggerButton() {
        Button button = currentlySelected.GetComponent<Button>();
        Debug.Log(currentlySelected);
        Debug.Log(button);

        if (button != null)
        {
            
            //if (timeout + 1 > Time.time) return;
            //notificationSystem.notify(other.gameObject.name, "EventC " + (button.onClick.GetPersistentEventCount()), 5);
            //timeout = Time.time;
            Debug.Log("INVOKIGN BUTTON!!!");

            button.onClick.Invoke();
            detailsStage = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (lastEvent + 0.5 > Time.time && rightHand.HandConfidence < OVRHand.TrackingConfidence.High) return;

        lastEvent = Time.time;

        if (other.gameObject.name == "LeftIncButton")
        {
            selectNext();
        }
        else if (other.gameObject.name == "RightDecButton")
        {
            selectPrev();
        }
        else if (other.gameObject.name == "ConfirmButton")
        {
            if (!detailsStage)
            {
                triggerButton();
            }
            else
            {
                startButtion.onClick.Invoke();
            }
        }
        else if (other.gameObject.name == "ReturnButton")
        {
            if (detailsStage)
            {
                detailsStage = false;
                returnButtion.onClick.Invoke();

            }
        }

            Debug.Log("Entered trigger with: " + other.name);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Staying inside trigger with: " + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger with: " + other.name);
    }
}
