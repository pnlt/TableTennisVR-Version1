using System;
using Dorkbots.XR.Runtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Runtime.UserInterface
{
    public class TimeNotification : MonoBehaviour
    {
        [Header("Reference components")] [SerializeField]
        private TextMeshProUGUI timeTxt;

        private void OnEnable()
        {
            DisplayTimerEvent.Subscribe(DisplayTime);   
        }

        private void DisplayTime(DisplayTimerData timerData)
        {
            var timeStr = TimeSpan.FromSeconds(timerData.ElapsedTime).ToString(@"mm\:ss");
            
            if (timerData.ElapsedTime <= 11)
                timeTxt.text = $"<color=red>{timeStr}</color>";
            else
                timeTxt.text = timeStr;
        }

        private void OnDisable()
        {
            DisplayTimerEvent.Unsubscribe(DisplayTime);
        }
    }
}