using _Project.Scripts.Runtime.Interfaces;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using Dorkbots.XR.Runtime;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
public class LevelSO : ScriptableObject, IScoreDecrease, IScoreIncrease, IDataPersistence
{
    public int levelNum;
    public Challenges respectiveChallenge;
    public int requiredScore; // Score needed to move to next level
    public bool overScore;
    public bool isUnlock;
    private float practiceScore;

    public void UpdateScore(GameManager gameManager)
    {
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
            ModeAlterationNotificationEvent.Invoke(new ModeNotificationData(true));
        }
    }

    public void ChallengeUpdate()
    {
        respectiveChallenge.IncreaseScore();
    }

    public void ScoreDecrease(GameManager gameManager, int satisfiedCondition, bool correctPose)
    {
        switch (levelNum)
        {
            case 1:
                gameManager.PlayerScore -= 0;
                break;
            case 2:
                ScoreRegulation(gameManager, satisfiedCondition, correctPose, 0);
                break;
            case 3:
                ScoreRegulation(gameManager, satisfiedCondition, correctPose, 0.5f);
                break;
        }
    }

    private void ScoreRegulation(GameManager gameManager, int satisfiedCondition, bool correctPose,
        float decreaseCoefficient)
    {
        if ((correctPose && satisfiedCondition == 1) || (!correctPose && satisfiedCondition == 2) ||
            (!correctPose && satisfiedCondition == 1))
        {
            gameManager.PlayerScore -= decreaseCoefficient;
        }
        else
        {
            gameManager.PlayerScore -= (decreaseCoefficient + 0.5f);
        }
    }

    public void LoadData(GameData gameData)
    {
    }

    public void SaveData(ref GameData gameData)
    {
    }
}