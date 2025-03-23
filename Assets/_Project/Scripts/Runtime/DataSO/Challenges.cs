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
        public event Action OnChallengeCompleted;

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

        public void ChallengeFulfillment()
        {
            // Display level up confirm notification
            
            // Disable countdown timer UI   
            
            // Upgrade level/Go back to practice mode
            OnChallengeCompleted?.Invoke();
            //GameManager.Instance.CurrentLevel.isUnlock = true;
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