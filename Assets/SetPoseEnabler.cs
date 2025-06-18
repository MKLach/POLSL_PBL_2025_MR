using UnityEngine;

public class SetPoseEnabler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAbstractionLayer.Instance.setPoseEnabler(this.GetComponent<PoseInput>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
