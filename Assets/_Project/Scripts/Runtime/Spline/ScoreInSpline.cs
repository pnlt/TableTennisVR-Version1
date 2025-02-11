using System;
using UnityEngine;

namespace Dorkbots.XR.Runtime.Spline
{
    public class ScoreInSpline : ScoreCalculation
    {
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = GameManager.Instance;
        }

        public override void CalculateScore()
        {
            
        }
    }
}