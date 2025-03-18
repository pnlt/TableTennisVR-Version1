using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dorkbots.XR.Runtime;
using UnityEngine;

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
        // Handle perfect line completion


        /// <summary>
        /// Reset to the initial state of guideline
        /// </summary>
        public virtual void ResetLine()
        {
            ResetConditionEvent.Invoke();
            StatsReset();
        }

        private void StatsReset()
        {
            if (_checkpoints)
                _checkpoints.ResetLineState();
    
            // Store the current cover lines before clearing
            var linesToReset = new List<CoverLine>(GuideLine.CoverLines);
    
            // Clear the static list
            GuideLine.RemoveAllElements();
    
            // Reset each line manually
            ResetLines(linesToReset);
        }

        
        private async void ResetLines(List<CoverLine> linesToReset)
        {
            try
            {
                foreach (var line in linesToReset)
                {
                    if (line != null)
                    {
                        line.EnablingObject(true);
                        await UniTask.Delay(TimeSpan.FromSeconds(_intervalTime));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

    }
}