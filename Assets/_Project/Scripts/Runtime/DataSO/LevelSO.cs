using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level Data")]
public class LevelSO : ScriptableObject
{
    public int levelNum;
    public Challenges respectiveChallenge;
    public float requiredScore;         // Score needed to move to next level
}
