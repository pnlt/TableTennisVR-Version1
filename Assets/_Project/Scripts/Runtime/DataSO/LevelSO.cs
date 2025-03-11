using _Project.Scripts.Runtime.Interfaces;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
public class LevelSO : ScriptableObject, IScoreDecrease, IScoreIncrease
{
    public int levelNum;
    public Challenges respectiveChallenge;
    public float requiredScore; // Score needed to move to next level

    public void UpdateScore(GameManager gameManager)
    {
        gameManager.PlayerScore += 1;

        if (gameManager.PlayerScore >= requiredScore)
        {
            // Move to Challenges

            // Display notification for player to choose if they want to move to challenge
            

            // If the answer is yes, game mode will be changed to challenge
            // If not, it's still in normal mode
        }
    }

    public void ScoreDecrease(GameManager gameManager, int satisfiedCondition, bool correctPose)
    {
        switch (levelNum)
        {
            case 1:
                gameManager.PlayerScore += 0;
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