using System;
using Dorkbots.XR.Runtime.RacketInteraction;
using UnityEngine;

public class SignalPoint : MonoBehaviour
{
    public CollisionDetection collisionDetection;

    public void OutOfRangeSignal() {
        collisionDetection.OutOfRange = true;
    }
}