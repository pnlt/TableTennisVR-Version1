using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Tests.Runtime.Test;
using Dorkbots.XR.Runtime;
using Dorkbots.XR.Runtime.Spline;
using UnityEngine;
using UnityEngine.Serialization;

public class Checkpoints : MonoBehaviour
{
    private List<SplineCheckpoints> checkpoints = new List<SplineCheckpoints>();
    private int _checkpointIdxComparison;
    private int _currentCheckpoint;
    private int _numberOfCheckpoints = 0;
    
    public int NumberOfCheckpoints => _numberOfCheckpoints;
    public List<SplineCheckpoints> ListCheckpoints => checkpoints;
    public ScoreInSpline lineScore;
    
    private void Awake()
    {
        _currentCheckpoint = 0;
        
        _numberOfCheckpoints = gameObject.transform.childCount;         // Get number of currently checkpoints in the line
        checkpoints = gameObject.GetComponentsInChildren<SplineCheckpoints>().ToList();
        lineScore = GetComponentInParent<ScoreInSpline>();
    }

    private void OnEnable()
    {
        LineAttainmentEvent.Subscribe(LineChecking);
    }

    private void Start()
    {
        checkpoints[0].IsInTurn = true;     // The first checkpoint is always in turn
    }

    private void LineChecking(LineDataEvent lineData)
    {
        if (lineData.checkpoint.IsInTurn)
        {
            NextTurn();
        }
        else
        {
            var failedLine = new FailedNotification(this);
            lineScore.SetCondition(false);
            failedLine.ResetLine();
        }
    }

    /// <summary>
    /// This func will handle logical events after
    /// player move the racket adhering to the line successfully
    /// </summary>
    private void LineAttainment()
    {
        var successfulLine = new SuccessfulNotification(this); 
        lineScore.SetCondition(true);
        successfulLine.ResetLine();
    }

    /// <summary>
    /// This func has a duty on moving to next checkpoint's turn
    /// </summary>
    private void NextTurn()
    {
        checkpoints[_currentCheckpoint].ResetCountDown();
        checkpoints[_currentCheckpoint].IsInTurn = false;
        _currentCheckpoint++;
        
        // If number of checkpoints the racket has passed is equal the total of checkpoint => reset state
        if (_currentCheckpoint == _numberOfCheckpoints)
        {
            LineAttainment();
            return;
        }
        
        checkpoints[_currentCheckpoint].IsInTurn = true;
        checkpoints[_currentCheckpoint].IsCountDown = true;
    }

    /// <summary>
    /// Reset the checkpoints state like its initial state
    /// </summary>
    public void ResetLineState()
    {
        _currentCheckpoint = 0;
        checkpoints[0].IsInTurn = true;

        for (int i = 1; i < checkpoints.Count; i++)
        {
            checkpoints[i].IsInTurn = false;
        }
    }

    private void OnDisable()
    {
        LineAttainmentEvent.Unsubscribe(LineChecking);
    }
}
