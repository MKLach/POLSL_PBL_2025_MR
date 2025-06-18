using Oculus.Interaction;
using UnityEngine;

public class PoseInput : MonoBehaviour
{
    [SerializeField]
    private GameObject selectorObject;

    private ISelector selector;
    private bool isSelected = false;
    private bool wasSelectedLastFrame = false;

    public bool IsDownThisFrame { get; set; }

    public bool IsUpThisFrame { get; set; }

    public bool selected { get; private set; }

    public int mouseButtonBinding;

    public float lastUsed = 0;

    public float timeout = 0.5f;


    void Awake()
    {
        if (selectorObject == null)
        {
            Debug.LogError("Selector GameObject is not assigned.");
            return;
        }

        selector = selectorObject.GetComponent<ISelector>();
        if (selector == null)
        {
            Debug.LogError("Selector GameObject does not have a component implementing ISelector.");
        }
    }

    void OnEnable()
    {
        if (selector != null)
        {
            selector.WhenSelected += OnSelected;
            selector.WhenUnselected += OnUnselected;
        }
    }

    void OnDisable()
    {
        if (selector != null)
        {
            selector.WhenSelected -= OnSelected;
            selector.WhenUnselected -= OnUnselected;
        }
        isSelected = false;
    }

    public void UpdateLogic()
    {
        IsDownThisFrame = !wasSelectedLastFrame && isSelected;
        IsUpThisFrame = wasSelectedLastFrame && !isSelected;
        selected = isSelected;

        wasSelectedLastFrame = isSelected;
        //Debug.Log(isSelected);

        //if (IsDownThisFrame)
        // {
        //     Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAA");
        // }
    }

    public bool checkAgainsLastUsed() {

        if (Time.time > lastUsed + timeout) { 
            lastUsed = Time.time;
            return true;
        }
        return false;

    }

    private void OnSelected() => isSelected = true;
    private void OnUnselected() => isSelected = false;
}
