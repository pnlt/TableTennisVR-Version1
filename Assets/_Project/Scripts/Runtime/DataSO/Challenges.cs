using System;
using UnityEngine;

namespace Dorkbots.XR.Runtime.DataSO
{
    [System.Serializable]
    public class Challenges : IDisposable
    {
        public int requiredScore;
        public float limitedTime;
        public float challengeScore;

        public Challenges()
        {
            Timer.OnTimerEnded += CheckedChallenge;
            challengeScore = 0;
        }

        private void OutTime(bool value)
        {
        }

        public void IncreaseScore()
        {
            // Save challenge score data
            challengeScore++;
            DisplayScoreEvent.Invoke(new ScoreData(challengeScore));
            ChallengeFulfillment();
        }

        private void ChallengeFulfillment()
        {
            // Display congratulation and confirm notification
            
            // Upgrade level/Go back to practice mode
            
            // Disable countdown timer UI   
        }

        private void CheckedChallenge()
        {
            // Disable score management - Player can not score anymore 
            
            // Display failed notification - Player has two options (face challenge again or move back to normal for more practices)
        }

        public void Dispose()
        {
            Timer.OnTimerEnded -= CheckedChallenge;
            GC.SuppressFinalize(this);
        }
    }
}