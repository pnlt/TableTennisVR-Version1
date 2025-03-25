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
        
        #region Score calculation

        public void IncreaseScore()
        {
            // Save challenge score data
            challengeScore++;
            DisplayScoreEvent.Invoke(new ScoreData(challengeScore));
            ChallengeFulfillment();
        }

        public void DecreaseScore(int levelNum, int satisfiedCondition, bool correctPose)
        {
            switch (levelNum)
            {
                case 1:
                    challengeScore -= 0;
                    break;
                case 2:
                    ScoreRegulation(satisfiedCondition, correctPose, 0);
                    break;
                case 3:
                    ScoreRegulation(satisfiedCondition, correctPose, 0.5f);
                    break;
            }
            
            DisplayScoreEvent.Invoke(new ScoreData(challengeScore));
        }

        private void ScoreRegulation(int satisfiedCondition, bool correctPose,
            float decreaseCoefficient)
        {
            if ((correctPose && satisfiedCondition == 1) || (!correctPose && satisfiedCondition == 2) ||
                (!correctPose && satisfiedCondition == 1))
            {
                challengeScore -= decreaseCoefficient;
            }
            else
            {
                challengeScore -= (decreaseCoefficient + 0.5f);
            }
        }
        
        #endregion

        public void ChallengeFulfillment()
        {
            if (challengeScore >= requiredScore)
            {
                // TODO - Display level up confirm notification

                // TODO - Disable countdown timer UI   

                // TODO - Upgrade level/Go back to practice mode
                OnChallengeCompleted?.Invoke();

                // TODO - Transition to menu scene
            }
        }

        private void CheckedChallenge()
        {
            // TODO - Disable score management - Player can not score anymore 

            // TODO - Display failed notification - Player has two options (face challenge again or move back to normal for more practices)
        }

        public void Dispose()
        {
            Timer.OnTimerEnded -= CheckedChallenge;
            GC.SuppressFinalize(this);
        }
    }
}