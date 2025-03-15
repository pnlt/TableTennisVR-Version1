using System;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class ModeNotification : MonoBehaviour
{
    private void OnEnable()
    {
        ModeAlterationNotificationEvent.Subscribe(ActivateNotification);
    }
    
    private void ActivateNotification(ModeNotificationData data)
    {
        gameObject.SetActive(data.Flag);
    }

    private void OnDisable()
    {
        ModeAlterationNotificationEvent.Unsubscribe(ActivateNotification);
    }
}
