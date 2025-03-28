using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackPlayBtn : MonoBehaviour
{
    List<string> levelLabels = new() { "Level_1", "Level_2", "Level_3" };
    public void OnClick() {
        // TODO - Go back to play scene
        var currentLevelNum = GameManager.Instance.CurrentLevel.levelNum - 1;
        SceneManager.LoadSceneAsync(levelLabels[currentLevelNum]);
    }
}