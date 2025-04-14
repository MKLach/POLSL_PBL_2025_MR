using Unity.VisualScripting;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    public ChildTriggerForwarder forwarder;
    public int id;

    void OnTriggerEnter(Collider other)
    {
        forwarder.NotifyTriggerEnter(this.id, other);
    }

    void OnTriggerExit(Collider other)
    {
        forwarder.NotifyTriggerExit(this.id, other);
    }

    void OnTriggerStay(Collider other)
    {
        forwarder.NotifyTriggerStay(this.id, other);
    }
}