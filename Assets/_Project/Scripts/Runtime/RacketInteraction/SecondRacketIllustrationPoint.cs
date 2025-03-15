using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition)
    {
        if (!isCorrectPose)
        {
            scoreSampleRacket.SetCondition(false);
            return;
        }
        base.ConditionValidation(condition);
        UIManager.Instance.SetValueDebug("Second Racket");
        scoreSampleRacket.SetCondition(true);
    }
}