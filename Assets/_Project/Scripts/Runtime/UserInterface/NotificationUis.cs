using System;
using System.Collections;
using _Project.Scripts.Runtime.UserInterface;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUis : MonoBehaviour
{
    public GameObject modeNoti;
    public GameObject timeNoti;
    public GameObject finishNoti;
    [SerializeField] private float notificationDuration = 2f;
    public RadialSelectionMenu radialSelectionMenu;
    public int challengePartIndex = 3;

    private void OnEnable() {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        TimeNotificationEvent.Subscribe(TimeNotification);
        FinishNotificationEvent.Subscribe(FinishNotification);
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

    private void FinishNotification(FinishNotificationData data) {
        finishNoti.SetActive(data.Flag);
        if (data.Flag)
            StartCoroutine(DisableFinishNotificationAfterDelay());
    }

    private IEnumerator DisableFinishNotificationAfterDelay() {
        yield return new WaitForSeconds(notificationDuration);
        finishNoti.SetActive(false);
    }


    private void TimeNotification(TimeNotificationData data) {
        timeNoti.SetActive(data.Flag);
    }

    private void OnDisable() {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        TimeNotificationEvent.Unsubscribe(TimeNotification);
        FinishNotificationEvent.Unsubscribe(FinishNotification);
    }
}