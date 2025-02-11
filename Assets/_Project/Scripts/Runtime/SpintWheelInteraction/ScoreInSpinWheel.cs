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
        gameManager.PlayerScore += gainedScore;
    }
}

