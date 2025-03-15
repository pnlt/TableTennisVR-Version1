namespace Dorkbots.XR.Runtime.DataSO
{
    [System.Serializable]
    public class Challenges
    {
        public int requiredScore;
        public float limitedTime;

        public void IncreaseScore(GameManager gameManager)
        {
            gameManager.ChallengeScore += 1;
            // Save challenge score data

            if (gameManager.ChallengeScore >= requiredScore)
            {
                // Unlock new level
            }
        }
    }
}