using System;
using Michsky.UI.Heat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private ChapterManager chapterManager;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void ChooseLevel(int level)
    {
        gameManager.CurrentLevelIndex = level;
    }

    public void OnPlayPressed(string levelLabel)
    {
        SceneManager.LoadSceneAsync(levelLabel);
    }

    private void Start() {
        UpdateLevelStatus();
    }

    private void UpdateLevelStatus() {
        for (int i = 0; i < chapterManager.chapters.Count; i++)
        {
            string chapterId = chapterManager.chapters[i].chapterID;

            if (GameManager.Instance.IsLevelUnlocked(i))
            {
                ChapterManager.SetUnlocked(chapterId);
            }
            else
            {
                ChapterManager.SetLocked(chapterId);
            }
        }

        // Refresh UI
        chapterManager.InitializeChapters();
    }
}