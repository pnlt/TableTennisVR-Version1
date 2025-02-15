using System;
using UnityEngine;

public class ScoreInSpinWheel : ScoreCalculation
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public override void CalculateScore()
    {
        Debug.Log("Score wheel Calculated");
        gameManager.PlayerScore += gainedScore;
    }
}

