using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public GameObject notificationPrefab;
    public Camera camera;
    public NotificationComponent notification;
    float timeToFade = 0;
    public void notifyNotificationDestroyed(int id)
    {
        //if (id == this.id - 1) {
            //this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        //}
        //return;
        bool after = false;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < this.gameObject.transform.childCount; i++) { 
            GameObject go = this.gameObject.transform.GetChild(i).gameObject;
            
            NotificationComponent nc = go.GetComponent<NotificationComponent>();

            if(nc != null )
            {
                if (!after) {
                    if (nc.getId() == id) {
                        after = true;
                    }
                   
                } else {
                    go.transform.position += new Vector3(0, 0.22f, 0);
                }
            }
        }
    }

    int id = 0;
    public void notify(String title, String text, float time = 10f) {
        timeToFade = Time.time+ time;
        Debug.LogError(title + " " + text);
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        notification.init(title, text, id++, this, time);
  
    }

    void Start()
    {
        notify("Hello!", "Test notification!", 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToFade < Time.time) {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }
}
