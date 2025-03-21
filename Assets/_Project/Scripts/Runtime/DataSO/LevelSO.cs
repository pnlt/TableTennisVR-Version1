using _Project.Scripts.Runtime.Interfaces;
using Dorkbots.XR.Runtime;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
public class LevelSO : ScriptableObject, IScoreDecrease, IScoreIncrease
{
    public int levelNum;
    public Challenges respectiveChallenge;
    public int requiredScore; // Score needed to move to next level
    public bool overScore;

    public void UpdateScore(GameManager gameManager)
    {
        gameManager.PlayerScore += 1;
        gameManager.NormalScore += 1;
        // Save the normal score data for future upload
        
        // Display score on UI
        DisplayScoreEvent.Invoke(new ScoreData(gameManager.NormalScore));

        if (gameManager.PlayerScore >= requiredScore && !overScore)
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
}