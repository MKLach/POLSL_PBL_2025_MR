using UnityEngine;

public class StyleSingleton : MonoBehaviour
{
    public static StyleSingleton Instance;
    public Color textColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textColor = Color.green;

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
