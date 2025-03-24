using System;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

namespace _Project.Scripts.Runtime.Logics
{
    public class LevelController : MonoBehaviour
    {
        private void OnEnable()
        {
           GameManager.Instance.CurrentLevel.respectiveChallenge.OnChallengeCompleted += HandleChallengeCompleted;
        }

        private void HandleChallengeCompleted()
        {
            GameManager.Instance.CompleteCurrentLevel();
        }

        private void OnDisable()
        {
            if (GameManager.Instance.CurrentLevel.respectiveChallenge != null)
            {
                GameManager.Instance.CurrentLevel.respectiveChallenge.OnChallengeCompleted -= HandleChallengeCompleted;
            }
        }
    }
}