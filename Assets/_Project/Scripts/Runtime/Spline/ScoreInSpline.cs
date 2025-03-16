using System;
using UnityEngine;

namespace Dorkbots.XR.Runtime.Spline
{
    public class ScoreInSpline : BaseScoreCalculation
    {
        private void Update()
        {
            if (correctCondition)
                TestCheckpoint.listCheckUI[0].ChangeColor(Color.green);
        }

        public override void SetCondition(bool flag)
        {
            correctCondition = flag;
            if (correctCondition)
            {
                UIManager.Instance.SetValueDebug("Spline");
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