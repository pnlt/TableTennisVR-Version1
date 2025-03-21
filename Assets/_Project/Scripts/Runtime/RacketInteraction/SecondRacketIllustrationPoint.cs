using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition) {
        scoreSampleRacket.AddSampleRacket(this);
        if (!isCorrectPose)
        {
            UIManager.Instance.SetValueDebug($"nhu cc {condition}");
            scoreSampleRacket.SetCondition(false);
            return;
        }

        base.ConditionValidation(condition);
        UIManager.Instance.SetValueDebug($"Good to go {condition}");
        scoreSampleRacket.SetCondition(true);
    }

    public override void SetMatToOrigin() {
        racketRender.material = originalMat;
    }
}