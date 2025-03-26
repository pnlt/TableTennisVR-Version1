using System;
using System.Collections;
using _Project.Scripts.Runtime.UserInterface;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUIs : MonoBehaviour
{
    public GameObject modeNoti;
    public GameObject timeNoti;
    public Timer time;
    [SerializeField] private float notificationDuration = 2f;
    public RadialSelectionMenu radialSelectionMenu;
    public int challengePartIndex = 3;

    private void OnEnable() {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        TimeNotificationEvent.Subscribe(TimeNotification);
    }

    private void ModeActivation(ModeNotificationData data) {
        modeNoti.SetActive(data.Flag);
        if (data.Flag)
            StartCoroutine(DisableNotificationAfterDelay());
    }

    private IEnumerator DisableNotificationAfterDelay() {
        yield return new WaitForSeconds(notificationDuration);
        modeNoti.SetActive(false);
        EnableChallengePart();
    }

    private void EnableChallengePart() {
        if (radialSelectionMenu != null)
            radialSelectionMenu.EnableRadialPart(challengePartIndex);
    }


    private void TimeNotification(TimeNotificationData data) {
        timeNoti.SetActive(data.Flag);
        time.gameObject.SetActive(data.Flag);
    }

    private void OnDisable() {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        TimeNotificationEvent.Unsubscribe(TimeNotification);
    }
}