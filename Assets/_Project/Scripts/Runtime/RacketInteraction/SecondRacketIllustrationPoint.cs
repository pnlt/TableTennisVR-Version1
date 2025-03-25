using Dorkbots.XR.Runtime.SoundAndSFX;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    public override void ConditionValidation(bool condition) {
        scoreSampleRacket.AddSampleRacket(this);
        if (!isCorrectPose)
        {
            scoreSampleRacket.SetCondition(false);
            return;
        }

        scoreSampleRacket.SetCondition(condition);
    }

    public override void SetMatToOrigin() {
        racketRender.material = originalMat;
    }
    
}