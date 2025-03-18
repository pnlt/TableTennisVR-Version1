using System;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUIs : MonoBehaviour
{
    public GameObject modeNoti;
    public GameObject timeNoti;
    
    private void OnEnable()
    {
        GameActivationEvent.Subscribe(ActivateModeNotification);
    }
    
    private void ActivateModeNotification(GameActivationData objData)
    {
        objData.gameObj.SetActive(objData.Flag);
    }

    private void OnDisable()
    {
        GameActivationEvent.Unsubscribe(ActivateModeNotification);
    }
}
