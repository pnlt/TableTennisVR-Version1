using System;
using System.Collections.Generic;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using Dorkbots.XR.Runtime.DataSO;
using Dreamteck.Splines.Primitives;
using UnityEngine;

namespace _Project.Scripts.Runtime.Logics
{
    public class LevelController : MonoBehaviour
    {
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
           gameManager.CurrentLevel.respectiveChallenge.OnChallengeCompleted += HandleChallengeCompleted;
        }

        private void HandleChallengeCompleted()
        {
            GameManager.Instance.CompleteCurrentLevel();
        }

        private void OnDisable()
        {
            if (gameManager.CurrentLevel.respectiveChallenge != null)
            {
                gameManager.CurrentLevel.respectiveChallenge.OnChallengeCompleted -= HandleChallengeCompleted;
            }
        }
    }
}