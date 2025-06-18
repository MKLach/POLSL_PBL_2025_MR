using UnityEngine;

public class IALHandshake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAbstractionLayer.Instance.setPosesManager(this.gameObject);




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
