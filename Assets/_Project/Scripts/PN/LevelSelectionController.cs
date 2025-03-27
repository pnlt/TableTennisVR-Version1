using System;
using Michsky.UI.Heat;
using UnityEngine;

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
        gameManager.CurrentLevelIndex(level);
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
                /*if (i == GameManager.Instance.currentLevelIndex)
                {
                    ChapterManager.SetCurrent(chapterId);
                }
                else
                {
                    ChapterManager.SetUnlocked(chapterId);
                }*/
                ChapterManager.SetUnlocked(chapterId);
                //ChapterManager
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