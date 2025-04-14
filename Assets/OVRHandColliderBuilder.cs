using TMPro;
using UnityEngine;


public class OVRHandColliderBuilder : MonoBehaviour
{
    public OVRHand hand; // Assign in Inspector (Left or Right Hand)
    public float colliderRadius = 0.01f;
    public float colliderHeight = 0.05f;
    public bool showDebugGizmos = false;
    public TextMeshProUGUI textMeshPro;
    
    public ChildTriggerForwarder recalc;
    void Start()
    {


        //textMeshPro.text = $"Debug {hand.name}\n";
        if (hand == null)
        {
            Debug.LogError("OVRHand reference not assigned!");
            //textMeshPro.text += "Errot\n";
            return;
        }

        BuildColliders();
        recalc.recalc();
    }

    void BuildColliders()
    {
        Transform[] joints = hand.GetComponentsInChildren<Transform>();
        int i = 0;
        foreach (Transform joint in joints)
        {
            // Avoid adding colliders to root / wrist
            if (joint == hand.transform) continue;

            if (!joint.name.Contains("IndexTip")) continue;

            // Skip if already has collider
            if (joint.GetComponent<Collider>() != null) continue;

            // Add Capsule Collider
            CapsuleCollider col = joint.gameObject.AddComponent<CapsuleCollider>();
            col.name = joint.name + " " + i;
            //textMeshPro.text += $"{col.name}\n";
            col.isTrigger = true;
            col.radius = colliderRadius;
            col.height = colliderHeight;
            col.direction = 2; // Z-axis

            // Add Rigidbody (Kinematic, so no physics forces)
            Rigidbody rb = joint.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            i++;
        }
        
        Debug.Log($"[OVRHandColliderBuilder] Added colliders to {joints.Length} joints of {hand} hand.");
    }

    // Optional: Draw colliders for debugging
    void OnDrawGizmos()
    {
        if (!showDebugGizmos || hand == null) return;

        Transform[] joints = hand.GetComponentsInChildren<Transform>();
        Gizmos.color = Color.green;
        foreach (Transform joint in joints)
        {
            Gizmos.DrawWireSphere(joint.position, colliderRadius);
        }
    }
}