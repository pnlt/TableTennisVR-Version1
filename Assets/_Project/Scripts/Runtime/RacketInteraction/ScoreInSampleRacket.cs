using UnityEngine;

public class ScoreInSampleRacket : BaseScoreCalculation
{
    protected override void ResetCondition()
    {
        if (!correctCondition)
        {
            // Notify the failure of posing sample racket
            return;
        }
        correctCondition = false;
    }
}
