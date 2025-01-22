using _Project.Scripts.Tests.Runtime.RacketInteraction;
using _Project.Scripts.Tests.Runtime.Test;
using UnityEngine;

public class InteractiveRacket : MonoBehaviour
{
    [Header("Interactive stat")]
    public Vector3 localCorePos;
    [Range(.2f, 10)]
    public float radius;

    public Vector3 racketVelocity;
    
    [Header("Detection layer")]
    public LayerMask interactLayer;
    
    [Header("References")]
    public Checkpoints checkPoints;

    private int _passedCheckPoints;
    private int numCheckPoints = 0;

    private void Start()
    {
        numCheckPoints = checkPoints.NumberOfCheckpoints;       // Grant number of available checkpoints
    }

    private void OnCollisionEnter(Collision collider)
    {
        var wheelLayer = 1 << collider.gameObject.layer;
        if (interactLayer == wheelLayer)
        {
            // Get contact point => collision point
            var contact = collider.contacts[0];
            
            // If collided object is spin wheel
            if (collider.gameObject.TryGetComponent<PhysicalWheel>(out var wheel))
            {
                wheel.RbRotationalMotion(racketVelocity, contact.point);    
            }
        }
    }

    /// <summary>
    /// Handling the detection with the checkpoint to check perfect line
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        // Get layer-mask of the collided game-object
        var checkpointLayer = 1 << collider.gameObject.layer;

        // If the collided game-object's layer-mask is same as checkpoint's layer-mask
        if (checkpointLayer == interactLayer)
        {
            Debug.Log("ngu dien");
            var splineCheckPoint = collider.GetComponent<SplineCheckpoints>();
            // See if that checkpoint is in turn or not
            if (splineCheckPoint.IsInTurn)
            {
                var checkPointManager = collider.GetComponentInParent<Checkpoints>();
                // Move to next checkpoint's turn
                _passedCheckPoints++;
                checkPointManager.NextTurn();
            }
            else    // FAILURE
            {
                var failedLine = new FailedNotification(checkPoints);
                _passedCheckPoints = 0;
                failedLine.ResetLine();
            }
        }
        
        AttainPerfectLine();
    }

    /// <summary>
    /// This func will handle logical events after
    /// player move the racket adhering to the line successfully
    /// </summary>
    private void AttainPerfectLine()
    {
        // If crossing over all the checkpoints in the line = perfect line     //SUCCESS
        if (_passedCheckPoints == numCheckPoints)
        {
            _passedCheckPoints = 0;
            var successfulLine = new SuccessfulNotification(checkPoints);
            successfulLine.ResetLine();
        }
    }
}
