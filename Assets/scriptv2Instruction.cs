using UnityEngine;
using UnityEngine.UI;

public class scriptv2Instruction : MonoBehaviour
{
    public GameObject parent_ui;
    public ChildTriggerForwarder handTrigger;
    public NotificationSystem notificationSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        Debug.Log("INIT v1" + this.gameObject.name);
        handTrigger.OnAnyTriggerEnter += (id,other) => OnTriggerEnter2(id,other); //Debug.Log("Hand touched: " + other.name);
        handTrigger.OnAnyTriggerExit += (id,other) => OnTriggerExit2(id,other);   // Debug.Log("Hand stopped touching: " + other.name);
        timeout = Time.time;
    }
    float timeout;

    bool[] touchers = new bool[10];

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < touchers.Length; i++) {
            touchers[i] = false;
        }
    }


    // Wykrywanie kolizji z innymi obiektami
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Zderzenie z: " + collision.gameObject.name);
    }

    bool isAnyOneTouching = false;

    bool isAnyTouching()
    {
        for( int i = 0;i < touchers.Length;i++) {
            if (touchers[i]) return true;
        }
        return false;
    }

    bool isAnyOneElseTouching(int id)
    {
        for (int i = 0; i < touchers.Length; i++)
        {
            if(i == id) continue;
            if (touchers[i]) return true;
        }
        return false;
    }

    // Wykrywanie wejœcia w trigger
    private void OnTriggerEnter2(int id, Collider other)
    {
        
        if (isAnyTouching()) { touchers[id] = true; return; }
       
        touchers[id] = true;

        Debug.Log("Obiekt " + other.gameObject.name + " wszed³ w trigger!");
        GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();
        if (other.gameObject.name == "FOLLOW_LOCKER") {
            G_F.lockGUIFollowing();
        }

        if (other.gameObject.name == "Cube (1)") {
           
           
            G_F.recalculate();
            G_F.toggleVisibility();
            
        }

        Button button = other.GetComponent<Button>();
        if (button != null) {
            if (timeout + 1 > Time.time) return;

            timeout = Time.time;
            notificationSystem.notify(other.gameObject.name, "Button pressed", 5);

            button.onClick.Invoke();

        }


    }

    // Wykrywanie wyjœcia z triggera
    private void OnTriggerExit2(int id, Collider other)
    {
        notificationSystem.notify(other.gameObject.name, "Trigger exit", 5);
        if (isAnyOneElseTouching(id)) { touchers[id] = false; return; }

        touchers[id] = false;


        Debug.Log("Obiekt " + other.gameObject.name + " opuœci³ trigger!");
        GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();
        if (other.gameObject.name == "FOLLOW_LOCKER")
        {
            G_F.unlockGUIFollowing();
        }
    }
}
