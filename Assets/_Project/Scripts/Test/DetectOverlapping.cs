using System;
using _Project.Scripts.Tests.Runtime.RacketInteraction;
using UnityEngine;

public class DetectOverlapping : MonoBehaviour
{
    public LayerMask wheelLayer;
    public Rigidbody cubeRb;

    private void Start()
    {
        //cubeRb.rotation = Quaternion.Euler(new Vector3(0f, 45f, 0f));
    }

    public void Update()
    {
        //transform.rotation = cubeRb.rotation;
        Debug.Log(cubeRb.rotation.eulerAngles);
    }
    /*private void OnCollisionEnter(Collision collision)
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

    }*/
}
