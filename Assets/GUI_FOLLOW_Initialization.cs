using UnityEngine;

public class GUI_FOLLOW_Initialization : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 rotationOffset;
    public Collider leftColllider;
    public Collider rightColllider;
    private bool lockFollowing = false;

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
        if (lockFollowing) return;
        transform.position = player.position + player.forward * offset.z +
                            player.right * offset.x +
                            player.up * offset.y;

        // Make the object face the player
        //transform.LookAt(player);
        transform.LookAt(player);

        transform.Rotate(-rotationOffset);

    }
    float alpha_og = 0.8f;

    public void toggleVisibility() {

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            if (cg) {
                if (cg.alpha != 0)
                {
                    alpha_og = cg.alpha;
                    cg.alpha = 0f;
                }
                else
                {
                    cg.alpha = alpha_og;
                }
                continue;
            }

            MeshRenderer mr = obj.GetComponent<MeshRenderer>();

            if (mr) {
                //mr.enabled = !mr.enabled;
                //Collider col = obj.GetComponent<Collider>();

                //col.enabled = !col.enabled;
            }

        }

    }

    public void changeVisibility(bool shouldBeVisible)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            

            GameObject obj = transform.GetChild(i).gameObject;
            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            if (cg)
            {
                if (shouldBeVisible)
                {

                    cg.alpha = alpha_og;
                    Debug.Log(cg.alpha);
                }
                else {
                    cg.alpha = 0;
                }
               
                     
                    
                
                

                continue;
            }
            
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();

            if (mr)
            {
                //mr.enabled = !mr.enabled;
                //Collider col = obj.GetComponent<Collider>();

                //col.enabled = !col.enabled;
            }

        }

    }


    public void lockGUIFollowing(){
        this.lockFollowing = true;   
    }

    public void unlockGUIFollowing()
    {
        this.lockFollowing = false;
    }




}
