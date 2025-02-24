using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Tests.Runtime.Test
{
    public class Notification
    {
        private readonly float
            _intervalTime = 0.2f; // An amount of time to display next spline cover line disabled

        private readonly Checkpoints _checkpoints;

        protected Notification(Checkpoints checkpoints)
        {
            _checkpoints = checkpoints;
        }

        private int _currentCoverLineIdx;

        /// <summary>
        /// Reset to the initial state of guideline
        /// </summary>
        public virtual void ResetLine()
        {
            EventBus<FinalScoreEvent>.Raise(new FinalScoreEvent());

            ResetInitialLine();
            VisualReset();
        }

        private void VisualReset()
        {
            _checkpoints.ResetLineState();
            GuideLine.CoverLines.Clear();
            _currentCoverLineIdx = 0;

            foreach (var item in TestCheckpoint.listCheckUI)
            {
                item.ChangeColor(Color.white);
            }
        }

        // Reset the line as its initial state
        private async void ResetInitialLine()
        {
            Debug.Log("Reset Initial Line");
            try
            {
                for (_currentCoverLineIdx = 0;
                     _currentCoverLineIdx < GuideLine.CoverLines.Count;
                     _currentCoverLineIdx++)
                {
                    GuideLine.CoverLines[_currentCoverLineIdx].EnablingObject(flag: true);
                    await UniTask.Delay(TimeSpan.FromSeconds(_intervalTime));
                }
            }
            catch (Exception e)
            {
                // ignored
                Debug.LogException(e);
            }
        }
    }
}