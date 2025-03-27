using UnityEngine;
using UnityEngine.SceneManagement;

public class BackPlayBtn : MonoBehaviour
{
    public void OnClick() {
        // TODO - Go back to play scene
        SceneManager.LoadSceneAsync("Level_1");
    }
}