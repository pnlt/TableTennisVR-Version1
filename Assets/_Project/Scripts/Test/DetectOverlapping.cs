using System;
using _Project.Scripts.Tests.Runtime.RacketInteraction;
using UnityEngine;

public class DetectOverlapping : MonoBehaviour
{
    public LayerMask wheelLayer;
    private void OnCollisionEnter(Collision collision)
    {
        var spinWheelLayer = 1 << collision.gameObject.layer;
        if (wheelLayer == spinWheelLayer)
        {
            // Get contact point => collision point
            var contact = collision.contacts[0];
            
            // If collided object is spin wheel
            if (collision.gameObject.TryGetComponent<PhysicalWheel>(out var wheel))
            {
                var force = new Vector3();
                wheel.RbRotationalMotion(force, contact.point);    
            }
        }

    }
}
