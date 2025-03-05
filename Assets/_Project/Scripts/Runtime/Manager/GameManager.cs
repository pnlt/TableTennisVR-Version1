using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Data")]
    [SerializeField] private float score;
    [SerializeField] private float coin;

    [Header("Level regulations")] 
    [SerializeField] private List<LevelSO> levels;
    
    public LevelSO CurrentLevel { get => currentLevel; set => currentLevel = value; }
    
    private LevelSO currentLevel;
    private int currentLevelIndex;
    
    public List<LevelSO> Levels { get => levels; private set => levels = value; } 

    private void Start()
    {
        levels = new List<LevelSO>();
        currentLevel = levels[0];
    }

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
