using UnityEngine;

public class AlwaysUp : MonoBehaviour
{
    public GameObject toCopyFrom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        this.transform.position = new Vector3(this.toCopyFrom.transform.position.x,this.transform.position.y, this.toCopyFrom.transform.position.z);

        // Reset rotation to always face World Up (Y+)
        //transform.up = Vector3.up;
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }
}
