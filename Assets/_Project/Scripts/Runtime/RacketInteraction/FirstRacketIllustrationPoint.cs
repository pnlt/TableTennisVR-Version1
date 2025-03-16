using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class FirstRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition)
    {
        base.ConditionValidation(condition);
        scoreSampleRacket.AddSampleRacket(this);
    }

    public override void SetMatToOrigin()
    {
        racketRender.material = originalMat;
    }
}

