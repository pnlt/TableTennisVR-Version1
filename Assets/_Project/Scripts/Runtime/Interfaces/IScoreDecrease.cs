namespace _Project.Scripts.Runtime.Interfaces
{
    public interface IScoreDecrease
    {
        void ScoreDecrease(GameManager gameManager, int satisfiedCond, bool poseChecking);
    }
}