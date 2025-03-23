using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public float playerCoin = 0;
    public List<bool> levelCompletionStatus = new();


    [Serializable]
    public struct Level
    {
        public bool overScore;
        public int levelNum;

        public Level(bool overScore, int levelNum)
        {
            this.overScore = overScore;
            this.levelNum = levelNum;
        }
    }
}
