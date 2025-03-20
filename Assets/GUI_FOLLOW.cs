using UnityEngine;

public class GUI_FOLLOW : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 rotationOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        recalculate();

        //Debug.Log(player.position);
    }

    public void recalculate() {
        transform.position = player.position + player.forward * offset.z +
                            player.right * offset.x +
                            player.up * offset.y;

        // Make the object face the player
        //transform.LookAt(player);
        transform.LookAt(player);

        transform.Rotate(-rotationOffset);

    }
}
