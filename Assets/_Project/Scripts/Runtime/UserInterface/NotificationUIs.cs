using System;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUIs : MonoBehaviour
{
    public GameObject modeNoti;
    public GameObject timeNoti;
    
    private void OnEnable()
    {
        ModeAlterationNotificationEvent.Subscribe(ActivateModeNotification);
        TimeNotificationEvent.Subscribe(ActivateTimeLimitation);
    }
    
    private void ActivateModeNotification(ModeNotificationData data)
    {
        modeNoti.SetActive(data.Flag);
    }

    private void ActivateTimeLimitation(TimeNotificationData timeData)
    {
        timeNoti.SetActive(timeData.Flag);
    }

    private void OnDisable()
    {
        ModeAlterationNotificationEvent.Unsubscribe(ActivateModeNotification);
        TimeNotificationEvent.Unsubscribe(ActivateTimeLimitation);
    }
}
