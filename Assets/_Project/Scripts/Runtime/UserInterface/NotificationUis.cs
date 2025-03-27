using System;
using System.Collections;
using _Project.Scripts.Runtime.UserInterface;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class NotificationUis : MonoBehaviour
{
    [SerializeField] private float notificationDuration = 2f;

    public GameObject modeNoti;
    public GameObject timeNoti;
    public GameObject finishNoti;
    public GameObject failedChallengeNoti;
    public RadialSelectionMenu radialSelectionMenu;
    public int challengePartIndex = 3;

    private void OnEnable() {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        FinishNotificationEvent.Subscribe(FinishNotification);
        TimeNotificationEvent.Subscribe(TimeNotificationState);
        FailedChallengeNotification.Subscribe(FailedChallengeNotificationState);
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

    private void FailedChallengeNotificationState(FailedChallengeNotificationData data) {
        failedChallengeNoti.SetActive(data.Flag);
    }

    private void TimeNotificationState(TimeNotificationData data) {
        timeNoti.SetActive(data.Flag);
    }

    private void OnDisable() {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        FinishNotificationEvent.Unsubscribe(FinishNotification);
        TimeNotificationEvent.Unsubscribe(TimeNotificationState);
        FailedChallengeNotification.Unsubscribe(FailedChallengeNotificationState);
    }
}