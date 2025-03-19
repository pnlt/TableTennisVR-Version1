using System;
using _Project.Scripts.Runtime.UserInterface;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUIs : MonoBehaviour
{
    public GameObject modeNoti;
    public GameObject timeNoti;
    public Timer time;

    private void OnEnable()
    {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        TimeNotificationEvent.Subscribe(TimeNotification);
    }

    private void ModeActivation(ModeNotificationData data)
    {
        modeNoti.SetActive(data.Flag);
    }

    private void TimeNotification(TimeNotificationData data)
    {
        timeNoti.SetActive(data.Flag);
        time.gameObject.SetActive(data.Flag);
    }
    
    private void OnDisable()
    {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        TimeNotificationEvent.Unsubscribe(TimeNotification);
    }
}
