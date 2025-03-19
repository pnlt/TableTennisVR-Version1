using UnityEngine;

namespace Dorkbots.XR.Runtime.DataSO
{
    [System.Serializable]
    public class Challenges
    {
        public int requiredScore;
        public float limitedTime;
        public bool _isTimeOut = false;

        public Challenges()
        {
            Timer.OnTimeOut += OutTime;
        }

        private void OutTime(bool value)
        {
            _isTimeOut = value;
        }

        public void IncreaseScore(GameManager gameManager)
        {
            gameManager.ChallengeScore += 1;
            // Save challenge score data

        }

        private void FinishChallenge(GameManager gameManager)
        {
            if (gameManager.ChallengeScore >= requiredScore)
            {
                // Unlock new level
                
            }
        }
    }
}