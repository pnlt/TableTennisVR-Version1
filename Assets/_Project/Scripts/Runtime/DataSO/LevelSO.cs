using System;
using _Project.Scripts.Runtime.Interfaces;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using Dorkbots.XR.Runtime;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
public class LevelSO : ScriptableObject, IScoreDecrease, IScoreIncrease
{
    public int levelNum;
    public Challenges respectiveChallenge;
    public int requiredScore; // Score needed to move to next level
    public bool overScore;
    public float practiceScore;

    private void OnEnable() {
        //LoadDataPlay();
    }

    public void LoadDataPlay() {
        if (GameManager.Instance.levelData.TryGetValue(levelNum, out var levelData))
        {
            overScore = levelData.overScore;
            practiceScore = levelData.practiceScore;
        }
    }

    public void UpdateScore(GameManager gameManager) {
        gameManager.PlayerScore += 1;

        if (!overScore)
        {
            practiceScore += 1;

            // Display score on UI
            DisplayScoreEvent.Invoke(new ScoreData(practiceScore));
        }

        if (practiceScore >= requiredScore && !overScore)
        {
            overScore = true;

            // Display notification for player to choose if they want to move to challenge and appear the button to show the notification again.
            //ModeAlterationNotificationEvent.Invoke(new ModeNotificationData(true));
        }
    }

    public void ChallengeUpdate() {
        respectiveChallenge.IncreaseScore();
    }

    public void ScoreDecrease(int satisfiedCondition, bool correctPose) {
        respectiveChallenge.DecreaseScore(levelNum, satisfiedCondition, correctPose);
    }
}