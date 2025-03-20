using System;
using Dorkbots.XR.Runtime.RacketInteraction;
using UnityEngine;

public class SignalPoint : MonoBehaviour
{
    private CollisionDetection collisionDetection;

    private void Awake() {
        collisionDetection = GetComponentInParent<CollisionDetection>();
    }
    
    public void OutOfRangeSignal() {
        //collisionDetection.OutOfRange = true;
    }
}
