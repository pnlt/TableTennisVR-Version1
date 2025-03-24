using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")] public GameObject mainMenu;

    [Header("Main Menu Buttons")] public Button startButton;
    public Button quitButton;
    public Button updateButton;


    // Start is called before the first frame update
    void Start() {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        updateButton.onClick.AddListener(ClearDataFile);
    }

    private void QuitGame() {
        Application.Quit();
    }

    private void StartGame() {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    private void ClearDataFile() {
        string path = Path.Combine(Application.persistentDataPath, "data");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void HideAll() {
        mainMenu.SetActive(false);
    }

    public void EnableMainMenu() {
        mainMenu.SetActive(true);
    }
}