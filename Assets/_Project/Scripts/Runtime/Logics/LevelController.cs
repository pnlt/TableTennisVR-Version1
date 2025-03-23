using System;
using Dorkbots.XR.Runtime.DataSO;
using UnityEngine;

namespace _Project.Scripts.Runtime.Logics
{
    public class LevelController : MonoBehaviour
    {
        private void Start()
        {
           GameManager.Instance.CurrentLevel.respectiveChallenge.OnChallengeCompleted += HandleChallengeCompleted;
        }

        private void HandleChallengeCompleted()
        {
            GameManager.Instance.CompleteCurrentLevel();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance.CurrentLevel.respectiveChallenge != null)
            {
                GameManager.Instance.CurrentLevel.respectiveChallenge.OnChallengeCompleted -= HandleChallengeCompleted;
            }
        }
    }
}