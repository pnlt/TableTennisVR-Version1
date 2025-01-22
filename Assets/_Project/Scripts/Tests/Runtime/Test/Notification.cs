using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Tests.Runtime.Test
{
    public class Notification
    {
        private readonly float _intervalTime = 0.2f;        // An amount of time to display next spline cover line being disabled
        private readonly Checkpoints _checkpoints;
        
        protected Notification(Checkpoints checkpoints)
        {
            _checkpoints = checkpoints;
        }

        private int _currentCoverLineIdx;
        /// <summary>
        /// Reset to the initial state of guideline
        /// </summary>
        public virtual async void ResetLine()
        {
            try
            {
                // Display Sound

                // Reset the line as its initial state
                for (_currentCoverLineIdx = 0;
                     _currentCoverLineIdx < GuideLine.CoverLines.Count;
                     _currentCoverLineIdx++)
                {
                    GuideLine.CoverLines[_currentCoverLineIdx].EnablingObject(flag: true);
                    await UniTask.Delay(TimeSpan.FromSeconds(_intervalTime));
                }

                _checkpoints.ResetLineState();
                GuideLine.CoverLines.Clear();
                Debug.Log(GuideLine.CoverLines.Count);
                _currentCoverLineIdx = 0;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}