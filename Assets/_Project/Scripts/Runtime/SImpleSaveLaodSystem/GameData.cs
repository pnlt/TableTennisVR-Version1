using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public float playerCoin = 0;
    public List<bool> levelCompletionStatus = new();
    public LevelData levels = new();


    [Serializable]
    public struct Level
    {
        public bool overScore;
        public float practiceScore;

        public Level(bool overScore, float practiceScore)
        {
            this.overScore = overScore;
            this.practiceScore = practiceScore;
        }
    }
    
    [Serializable]
    public sealed class LevelData : SerializableDictionary<int, Level> {}
}
