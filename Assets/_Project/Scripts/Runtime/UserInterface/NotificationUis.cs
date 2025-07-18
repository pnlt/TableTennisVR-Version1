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
    public GameObject pauseNoti;

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
        UpdateUIinteractionEvent.Subscribe(UpdateNotificationState);
        PauseNotificationEvent.Subscribe(PauseNotification);
        
        DisableCanvas();
    }

    private void PauseNotification(PauseNotificationData data)
    {
        pauseNoti.SetActive(data.Flag);
    }

    private void ModeActivation(ModeNotificationData data) {
        modeNoti.SetActive(data.Flag);
        UpdateNotificationState();
        if (data.Flag)
            StartCoroutine(DisableNotificationAfterDelay());
    }

    private IEnumerator DisableNotificationAfterDelay() {
        EnableChallengePart();
        yield return new WaitForSeconds(notificationDuration);
        modeNoti.SetActive(false);
        UpdateNotificationState();
    }

    private void EnableChallengePart() {
        PlayerPrefs.SetInt(IN_CHALLENGE + gameManager.CurrentLevelIndex, 1); // Ensure challenge not completed yet
        PlayerPrefs.Save();
    }

    private void FinishNotification(FinishNotificationData data) {
        finishNoti.SetActive(data.Flag);
        UpdateNotificationState();
        if (data.Flag)
            StartCoroutine(DisableFinishNotificationAfterDelay());
    }

    private IEnumerator DisableFinishNotificationAfterDelay() {
        yield return new WaitForSeconds(notificationDuration);
        finishNoti.SetActive(false);
        UpdateNotificationState();
    }

    private void FailedChallengeNotificationState(FailedChallengeNotificationData data) {
        failedChallengeNoti.SetActive(data.Flag);
        UpdateNotificationState();
        interactionArea.SetActive(true);
    }

    private void TimeNotificationState(TimeNotificationData data) {
        timeNoti.SetActive(data.Flag);
        UpdateNotificationState();
    }

    private void UpdateNotificationState() {
        isAnyNotificationActive = modeNoti.activeSelf || timeNoti.activeSelf || finishNoti.activeSelf ||
                                  failedChallengeNoti.activeSelf;

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
        UpdateUIinteractionEvent.Unsubscribe(UpdateNotificationState);
        PauseNotificationEvent.Unsubscribe(PauseNotification);
        DisableCanvas();
    }
}