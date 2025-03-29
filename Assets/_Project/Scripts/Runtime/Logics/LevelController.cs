using System;
using System.Collections.Generic;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

namespace _Project.Scripts.Runtime.Logics
{
    public class LevelController : MonoBehaviour
    {
        private GameManager gameManager;
        public List<LevelSO> levels;

        public RadialSelectionMenu radialSelectionMenu;
        private Challenges challenges;

        private void Awake() {
            gameManager = GameManager.Instance;
            levels = gameManager.Levels;
        }

        void Start() {
            if (radialSelectionMenu == null)
            {
                radialSelectionMenu = FindObjectOfType<RadialSelectionMenu>(); // Fallback if not assigned in Inspector
            }
        }

        private void OnEnable() {
            gameManager.CurrentLevel.respectiveChallenge.OnChallengeCompleted += HandleChallengeCompleted;
            foreach (var level in levels)
            {
                LoadLevelData(level);
            }
        }

        private void LoadLevelData(LevelSO level) {
            if (GameManager.Instance.levelData.TryGetValue(level.levelNum, out var levelData))
            {
                level.overScore = levelData.overScore;
                level.practiceScore = levelData.practiceScore;
            }
        }


        private void HandleChallengeCompleted() {
            gameManager.CompleteCurrentLevel();
        }

        private void OnDisable() {
            if (gameManager.CurrentLevel.respectiveChallenge != null)
            {
                gameManager.CurrentLevel.respectiveChallenge.OnChallengeCompleted -= HandleChallengeCompleted;
            }
        }
    }
}