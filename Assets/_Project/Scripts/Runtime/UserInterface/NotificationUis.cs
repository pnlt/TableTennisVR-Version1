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

    [SerializeField] private Canvas notificationCanvas;
    private bool isAnyNotificationActive = false;
    [SerializeField] private GameObject interactionArea;

    private void Awake() {
        notificationCanvas = GetComponent<Canvas>();
        gameManager = GameManager.Instance;
    }

    private GameManager gameManager;
    private const string IN_CHALLENGE = "InChallenge_Level";

    private void OnEnable() {
        ModeAlterationNotificationEvent.Subscribe(ModeActivation);
        FinishNotificationEvent.Subscribe(FinishNotification);
        TimeNotificationEvent.Subscribe(TimeNotificationState);
        FailedChallengeNotification.Subscribe(FailedChallengeNotificationState);
        DisableCanvas();
    }

    private void ModeActivation(ModeNotificationData data) {
        modeNoti.SetActive(data.Flag);
        if (data.Flag)
            StartCoroutine(DisableNotificationAfterDelay());
    }

    private IEnumerator DisableNotificationAfterDelay() {
        EnableChallengePart();
        yield return new WaitForSeconds(notificationDuration);
        modeNoti.SetActive(false);
    }

    private void EnableChallengePart() {
        PlayerPrefs.SetInt(IN_CHALLENGE + gameManager.CurrentLevelIndex, 1); // Ensure challenge not completed yet
        PlayerPrefs.Save();
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
        UpdateNotificationState(data.Flag);
    }

    private void TimeNotificationState(TimeNotificationData data) {
        timeNoti.SetActive(data.Flag);
    }

    private void UpdateNotificationState(bool notificationActivated) {
        isAnyNotificationActive = failedChallengeNoti.activeSelf;

        if (isAnyNotificationActive)
        {
            EnableCanvas();
        }
        else
        {
            DisableCanvas();
        }
    }

    private void EnableCanvas() {
        if (notificationCanvas != null)
        {
            notificationCanvas.enabled = true;
            interactionArea.SetActive(true);
        }
    }

    private void DisableCanvas() {
        if (notificationCanvas != null)
        {
            notificationCanvas.enabled = false;
            interactionArea.SetActive(false);
        }
    }

    private void OnDisable() {
        ModeAlterationNotificationEvent.Unsubscribe(ModeActivation);
        FinishNotificationEvent.Unsubscribe(FinishNotification);
        TimeNotificationEvent.Unsubscribe(TimeNotificationState);
        FailedChallengeNotification.Unsubscribe(FailedChallengeNotificationState);
        DisableCanvas();
    }
}