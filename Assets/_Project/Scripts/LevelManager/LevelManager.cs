using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int levelNumber;

    void Awake() {
        Instance = this;
    }
}