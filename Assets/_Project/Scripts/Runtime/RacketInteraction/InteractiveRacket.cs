using _Project.Scripts.Tests.Runtime.RacketInteraction;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class InteractiveRacket : MonoBehaviour
{
    [Header("Detection layer")]
    public LayerMask interactLayer;
    public LayerMask wheelLayer;
    
    [Header("References")]
    public PaddleControl control;

    private void Start()
    {
        control = GetComponent<PaddleControl>();
    }

    #region Wheel Interaction

    private void OnCollisionEnter(Collision collision)
    {
        var spinWheelLayer = 1 << collision.gameObject.layer;
        if (wheelLayer == spinWheelLayer)
        {
            // If collided object is spin wheel
            if (collision.gameObject.TryGetComponent<PhysicalWheel>(out var wheel))
            {
                var contact = collision.contacts[0];                    // Get contact point => collision point when colliding
                
                var force = control.ForceApplied();
                wheel.ClaudeRotation(force, contact.point);    
            }
        }
    }

    #endregion
    
    #region Line Interaction
    
    /// <summary>
    /// Handling the detection with the checkpoint to check perfect line
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        // Get layer-mask of the collided game-object
        var checkpointLayer = 1 << collider.gameObject.layer;
        
        // If collided object is on right area
        if (collider.gameObject.TryGetComponent<ScoreInSpinWheel>(out var spinWheel))
        {
            spinWheel.SetCondition(true);
        }

        // If the collided game-object's layer-mask is same as checkpoint's layer-mask
        if (checkpointLayer == interactLayer)
        {
            SplineCheckpoints splineCheckpoints = collider.gameObject.GetComponent<SplineCheckpoints>();
            LineAttainmentEvent.Invoke(new LineDataEvent(splineCheckpoints));
        }
        
        // If collided object is the sign of out of range
        if (collider.TryGetComponent<SignalPoint>(out var signalPoint))
        {
            signalPoint.OutOfRangeSignal();
        }
    }
    
    #endregion
}
