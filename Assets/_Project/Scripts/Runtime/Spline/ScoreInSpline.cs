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

        protected override void ResetCondition()
        {
            if (!correctCondition)
            {
                Debug.Log("Not perfect line");
                //UIManager.Instance.SetValueDebug("Not perfect line");
                return;
            }
            correctCondition = false;
        }
    }
}