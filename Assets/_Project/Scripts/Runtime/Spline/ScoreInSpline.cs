using System;
using UnityEngine;

namespace Dorkbots.XR.Runtime.Spline
{
    public class ScoreInSpline : BaseScoreCalculation
    {
        public override void SetCondition(bool flag)
        {
            correctCondition = flag;
            if (correctCondition)
            {
                EventBus<ConditionActivatedEvent>.Raise(new ConditionActivatedEvent());
            }
        }

        protected override void ResetCondition()
        {
            if (!correctCondition)
            {
                Debug.Log("Not perfect line");
                return;
            }
            correctCondition = false;
        }
    }
}