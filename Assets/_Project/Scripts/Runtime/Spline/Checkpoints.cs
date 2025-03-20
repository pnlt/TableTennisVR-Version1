using System;
using System.Collections.Generic;
using System.Linq;
using Dorkbots.XR.Runtime;
using Dorkbots.XR.Runtime.Spline;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private List<SplineCheckpoint> checkpoints = new List<SplineCheckpoint>();
    private int _currentCheckpoint;
    private int _numberOfCheckpoints;
    
    public int NumberOfCheckpoints => _numberOfCheckpoints;
    public List<SplineCheckpoint> ListCheckpoints => checkpoints;
    public ScoreInSpline lineScore;
    
    private void Awake()
    {
        _currentCheckpoint = 0;
        
        _numberOfCheckpoints = gameObject.transform.childCount;         // Get number of currently checkpoints in the line
        checkpoints = gameObject.GetComponentsInChildren<SplineCheckpoint>().ToList();
        lineScore = GetComponentInParent<ScoreInSpline>();
    }

    private void OnEnable()
    {
        LineAttainmentEvent.Subscribe(LineChecking);
    }

    private void LineChecking(LineData lineData)
    {
        if (lineData.Checkpoint.IsInTurn)
        {
            NextTurn(lineData.Checkpoint);
        }
    }

    /// <summary>
    /// This func will handle logical events after
    /// player move the racket adhering to the line successfully
    /// </summary>
    private void LineAttainment(bool satisfiedCondition)
    {
        lineScore.SetCondition(satisfiedCondition);
        EventBus<CheckingConditionEvent>.Raise(new CheckingConditionEvent(this));
    }

    /// <summary>
    /// This func has a duty on moving to next checkpoint's turn
    /// </summary>
    private void NextTurn(SplineCheckpoint checkpoint)
    {
        var checkpointIdx = checkpoints.IndexOf(checkpoint);
        checkpoint.IsInTurn = false;
        checkpoint.ResetCountDown();
        
        if (checkpointIdx != _currentCheckpoint)
        {
            LineAttainment(false);
            return;
        }
        
        _currentCheckpoint += 1;
        
        // If number of checkpoints the racket has passed is equal the total of checkpoint => reset state
        if (_currentCheckpoint == _numberOfCheckpoints)
        {
            LineAttainment(true);
        }
        else
            checkpoints[_currentCheckpoint].IsCountDown = true;
    }

    /// <summary>
    /// Reset the checkpoints state like its initial state
    /// </summary>
    public void ResetLineState()
    {
        _currentCheckpoint = 0;

        foreach (var check in checkpoints)
        {
            check.IsInTurn = true;
        }
    }

    private void OnDisable()
    {
        LineAttainmentEvent.Unsubscribe(LineChecking);
    }
}
