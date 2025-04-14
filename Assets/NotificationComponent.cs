using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationComponent : MonoBehaviour
{
    private string title = "Oh no!";
    private string content = "You have forgotten to call init() with arguments!";
    private Time startTime;
    private NotificationSystem parent;
    private int id;
    private float time = 10f;
    public void init(string title, string content, int id, NotificationSystem parent, float time=10f) { 
        this.title = title;
        this.content = content;
        this.parent = parent;
        this.id = id;
        this.time = time;

        Start();

    }
    void Start()
    {
        //init("kappa", "adfawsnfaopfas", 0, null);

        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();

        tmp.text = title+"\n"+content;
        StartCoroutine(SelfDestruct());
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(time);
        parent.notifyNotificationDestroyed(id);
        
    }

    void Update()
    {
        
    }

    public Time getStartTime() { 
        return startTime;
    }

    public int getId() { return id; }
}
