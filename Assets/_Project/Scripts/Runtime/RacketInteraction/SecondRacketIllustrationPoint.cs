using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition)
    {
        scoreSampleRacket.AddSampleRacket(this);
        if (!isCorrectPose)
        {
            scoreSampleRacket.SetCondition(false);
            return;
        }
        base.ConditionValidation(condition);
        scoreSampleRacket.SetCondition(true);
    }
}