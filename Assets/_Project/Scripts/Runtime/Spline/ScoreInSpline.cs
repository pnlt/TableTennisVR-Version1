using System;
using UnityEngine;

namespace Dorkbots.XR.Runtime.Spline
{
    public class ScoreInSpline : BaseScoreCalculation
    {
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = GameManager.Instance;
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