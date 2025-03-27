using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private const string TUTORIAL_PLAYED_KEY = "HasPlayedTutorial";

    private void Awake() {
        if (!PlayerPrefs.HasKey(TUTORIAL_PLAYED_KEY))
        {
            PlayerPrefs.SetInt(TUTORIAL_PLAYED_KEY, 0);
        }
    }

    public async void OnPlayButtonPressed() {
        string sceneToLoad = PlayerPrefs.GetInt(TUTORIAL_PLAYED_KEY, 0) == 0 ? "Tutorial" : "Level_1";
        if (sceneToLoad == "Tutorial")
        {
            PlayerPrefs.SetInt(TUTORIAL_PLAYED_KEY, 1);
            PlayerPrefs.Save();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }

    public static void ResetTutorial() {
        PlayerPrefs.SetInt(TUTORIAL_PLAYED_KEY, 0);
        PlayerPrefs.Save();
    }
}