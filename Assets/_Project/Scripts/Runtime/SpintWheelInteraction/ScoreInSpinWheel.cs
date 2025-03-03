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
        if (!correctCondition)
        {
            Debug.Log("Did not hit right spin wheel area");
            return;
        }
        correctCondition = false;
    }
}

