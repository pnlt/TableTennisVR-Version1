using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class FirstRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition) {
        scoreSampleRacket.AddSampleRacket(this);
        base.ConditionValidation(condition: condition);
    }

    public override void SetMatToOrigin() {
        racketRender.material = originalMat;
    }
}