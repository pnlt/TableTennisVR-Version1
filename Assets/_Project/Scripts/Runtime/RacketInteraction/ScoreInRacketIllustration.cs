namespace Dorkbots.XR.Runtime.RacketInteraction
{
    public class ScoreInRacketIllustration : BaseScoreCalculation
    {
        protected override void ResetCondition()
        {
            if (!correctCondition)
            {
                UIManager.Instance.SetValueDebug("wrong sample racket's pose");
            }
            correctCondition = false;
        }
    }
}