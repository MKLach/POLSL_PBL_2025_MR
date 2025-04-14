using UnityEngine;
using System;

public class ChildTriggerForwarder : MonoBehaviour
{
    public event Action<int, Collider> OnAnyTriggerEnter;
    public event Action<int, Collider> OnAnyTriggerExit;
    public event Action<int, Collider> OnAnyTriggerStay;

    void Start()
    {
        // Add trigger listeners to all child colliders
        
    }

    public void recalc()
    {
        int id = 0;
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if (!col.isTrigger)
            {
                col.isTrigger = true; // Make sure it's trigger
            }
            var triggerListener = col.gameObject.AddComponent<TriggerListener>();
            triggerListener.forwarder = this;
            triggerListener.id = id;
            id++;
        }
    }

    // These get called by the TriggerListener
    public void NotifyTriggerEnter(int id, Collider other) => OnAnyTriggerEnter?.Invoke(id, other);
    public void NotifyTriggerExit(int id, Collider other) => OnAnyTriggerExit?.Invoke(id, other);
    public void NotifyTriggerStay(int id, Collider other) => OnAnyTriggerStay?.Invoke(id, other);
}