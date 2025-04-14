using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FistDetector : MonoBehaviour
{
    public OVRHand hand;
    public NotificationSystem notificationSystem;

    [UnityEngine.Range(0, 1)] public float fistThreshold = 0.9f;
    [UnityEngine.Range(0, 1)] public float openHandThreshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    bool isFist = false;
    // Update is called once per frame
    void Update()
    {
        isFist=(IsFist(hand));
 
    }

    bool IsFist(OVRHand hand)
    {
        // Check all finger flex values
        bool index = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > fistThreshold;
        bool middle = hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) > fistThreshold;
        bool ring = hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) > fistThreshold;
        bool pinky = hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) > fistThreshold;
        
        return index && middle && ring && pinky;
    }
    public float getThumb() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
    }
    public float getIndex() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
    }public float getMiddleb() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
    }public float getRing() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
    }public float getPinky() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);
    }

    public bool isFistGesture() {
        return isFist;
    }
}
