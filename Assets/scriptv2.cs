using UnityEngine;

public class scriptv2 : MonoBehaviour
{
    public GameObject parent_ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    // Wykrywanie kolizji z innymi obiektami
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Zderzenie z: " + collision.gameObject.name);
    }

    // Wykrywanie wejœcia w trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Obiekt " + other.gameObject.name + " wszed³ w trigger!");

        if (other.gameObject.name == "Cube (1)") {
           
            GUI_FOLLOW G_F = parent_ui.GetComponent<GUI_FOLLOW>();
            G_F.recalculate();
            G_F.toggleVisibility();
            
        }




    }

    // Wykrywanie wyjœcia z triggera
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Obiekt " + other.gameObject.name + " opuœci³ trigger!");
    }
}
