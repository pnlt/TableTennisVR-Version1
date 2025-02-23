using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float score;

    public float PlayerScore
    {
        get => score;
        set
        {
            if (score >= 0) score = value;
            else Debug.LogWarning("Score out of range");
        }
    }
}
