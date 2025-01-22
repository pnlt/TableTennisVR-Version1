using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private List<SplineCheckpoints> checkpoints = new List<SplineCheckpoints>();
    private int _currentCheckpoint;
    private int _numberOfCheckpoints = 0;
    
    public int NumberOfCheckpoints => _numberOfCheckpoints;
    
    private void Awake()
    {
        _currentCheckpoint = 0;
        
        _numberOfCheckpoints = gameObject.transform.childCount;         // Get number of currently checkpoints in the line
        checkpoints = gameObject.GetComponentsInChildren<SplineCheckpoints>().ToList();
    }

    private void Start()
    {
        checkpoints[0].IsInTurn = true;     // The first checkpoint is always in turn
    }

    /// <summary>
    /// This func has a duty on moving to next checkpoint's turn
    /// </summary>
    public void NextTurn()
    {
        checkpoints[_currentCheckpoint].IsInTurn = false;
        _currentCheckpoint++;
        
        // If number of checkpoints the racket has passed is equal the total of checkpoint => reset state
        if (_currentCheckpoint == _numberOfCheckpoints)
        {
            _currentCheckpoint = 0;
            checkpoints[_currentCheckpoint].IsInTurn = true;
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
        checkpoints[_currentCheckpoint].IsInTurn = true;

        for (int i = 1; i < checkpoints.Count; i++)
        {
            checkpoints[i].IsInTurn = false;
        }
    }
}
