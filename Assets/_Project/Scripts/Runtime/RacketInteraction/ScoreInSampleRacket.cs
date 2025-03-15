using UnityEngine;

public class ScoreInSampleRacket : BaseScoreCalculation
{
    [SerializeField] private ScoreManagement scoreManagement;

    public override void SetCondition(bool flag)
    {
        correctCondition = flag;
        scoreManagement.CorrectPose = correctCondition;
        if (correctCondition)
        {
            UIManager.Instance.SetValueDebug("SampleRacket");
        }
    }

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
