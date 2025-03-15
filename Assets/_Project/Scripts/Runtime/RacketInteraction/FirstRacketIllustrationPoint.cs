using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class FirstRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition)
    {
        base.ConditionValidation(condition);
        if (isCorrectPose)
            UIManager.Instance.SetValueDebug("First racket");
    }
}

