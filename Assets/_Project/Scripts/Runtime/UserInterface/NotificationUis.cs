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

    private GameManager gameManager;
    private const string IN_CHALLENGE = "InChallenge_Level";

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        FinishNotificationEvent.Subscribe(FinishNotification);
        TimeNotificationEvent.Subscribe(TimeNotificationState);
        FailedChallengeNotification.Subscribe(FailedChallengeNotificationState);
    }

    private void ModeActivation(ModeNotificationData data)
    {
        modeNoti.SetActive(data.Flag);
        if (data.Flag)
            StartCoroutine(DisableNotificationAfterDelay());
    }

    private IEnumerator DisableNotificationAfterDelay()
    {
        EnableChallengePart();
        yield return new WaitForSeconds(notificationDuration);
        modeNoti.SetActive(false);
    }


    private void EnableChallengePart()
    {
        PlayerPrefs.SetInt(IN_CHALLENGE + gameManager.CurrentLevelIndex, 1); // Ensure challenge not completed yet
        PlayerPrefs.Save();
    }

    private void FinishNotification(FinishNotificationData data)
    {
        finishNoti.SetActive(data.Flag);
        if (data.Flag)
            StartCoroutine(DisableFinishNotificationAfterDelay());
    }

    private IEnumerator DisableFinishNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDuration);
        finishNoti.SetActive(false);
    }

    private void FailedChallengeNotificationState(FailedChallengeNotificationData data)
    {
        failedChallengeNoti.SetActive(data.Flag);
    }

    private void TimeNotificationState(TimeNotificationData data)
    {
        timeNoti.SetActive(data.Flag);
    }

    private void OnDisable()
    {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        FinishNotificationEvent.Unsubscribe(FinishNotification);
        TimeNotificationEvent.Unsubscribe(TimeNotificationState);
        FailedChallengeNotification.Unsubscribe(FailedChallengeNotificationState);
    }
}