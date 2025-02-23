using UnityEngine;

public class ScoreInSpinWheel : BaseScoreCalculation
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    protected override void ResetCondition()
    {
        correctCondition = false;
    }
}

