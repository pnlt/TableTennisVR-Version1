using _Project.Scripts.Tests.Runtime.RacketInteraction;
using _Project.Scripts.Tests.Runtime.Test;
using Dorkbots.XR.Runtime.Spline;
using UnityEngine;

public class InteractiveRacket : MonoBehaviour
{
    [Header("Detection layer")]
    public LayerMask interactLayer;
    public LayerMask wheelLayer;
    
    [Header("References")]
    public Checkpoints checkPoints;
    public PaddleControl control;

    private int _passedCheckPoints;
    private int numCheckPoints = 0;

    private void Start()
    {
        numCheckPoints = checkPoints.NumberOfCheckpoints;       // Grant number of available checkpoints
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
        ScoreInSpline spline = null;

        // If the collided game-object's layer-mask is same as checkpoint's layer-mask
        if (checkpointLayer == interactLayer)
        {
            var splineCheckPoint = collider.GetComponent<SplineCheckpoints>();
            spline = collider.gameObject.transform.root.GetComponent<ScoreInSpline>();
            if (splineCheckPoint.IsInTurn)      //SUCCESS
            {
                var checkPointManager = collider.GetComponentInParent<Checkpoints>();
                _passedCheckPoints++;
                checkPointManager.NextTurn();
            }
            else if (!splineCheckPoint.IsInTurn)    // FAILURE
            {
                var failedLine = new FailedNotification(checkPoints);
                _passedCheckPoints = 0;
                spline.SetCondition(false);
                failedLine.ResetLine();
            }
        }
        
        // If collided object is on right area
        if (collider.gameObject.TryGetComponent<ScoreInSpinWheel>(out var spinWheel))
        {
            spinWheel.SetCondition(true);
        }
        
        AttainPerfectLine(spline);
    }

    /// <summary>
    /// This func will handle logical events after
    /// player move the racket adhering to the line successfully
    /// </summary>
    private void AttainPerfectLine(ScoreInSpline spline)
    {
        // If crossing over all the checkpoints in the line = perfect line     //SUCCESS
        if (_passedCheckPoints == numCheckPoints)
        {
            if (spline)
            {
                _passedCheckPoints = 0;
                var successfulLine = new SuccessfulNotification(checkPoints);
                spline.SetCondition(true);
                successfulLine.ResetLine();
            }
            else
            {
                Debug.Log("Error, can not find out spline object");
            }
        }
        
    }
    
    #endregion
}
