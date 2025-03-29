using System;
using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using Michsky.UI.Heat;
using UnityEngine;

public class ChallengePathTrigger : MonoBehaviour, IPathTrigger
{
    [SerializeField] private bool isEnabled = false;
    [SerializeField] private UIGradient sampleGradient;

    private const string IN_CHALLENGE = "InChallenge_Level";

    private UIGradient originalGradient;
    private Gradient initialGradient = null;
    private float initialOffset;
    private float initialZoom;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        originalGradient = GetComponent<UIGradient>();
    }

    private void OnEnable()
    {
        // TODO - Set IsEnable value to determine the exact color for the button          
        bool challenge = PlayerPrefs.GetInt(IN_CHALLENGE + gameManager.CurrentLevelIndex, 0) == 1;
        IsEnabled = challenge;
    }

    private void Start() {
        initialGradient = originalGradient.EffectGradient;
        initialOffset = originalGradient.Offset;
        initialZoom = originalGradient.Zoom;
    }

    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            // Update the visual state when enabled/disabled
            SetBtnColor(isEnabled);
        }
    }

    private void SetBtnColor(bool isEnabled) {
        if (isEnabled)
        {
            originalGradient.EffectGradient = sampleGradient.EffectGradient;
            originalGradient.Offset = sampleGradient.Offset;
            originalGradient.Zoom = sampleGradient.Zoom;
        }
        else if (initialGradient != null)
        {
            originalGradient.EffectGradient = initialGradient;
            originalGradient.Offset = initialOffset;
            originalGradient.Zoom = initialZoom;
        }
    }

    public void OnPathTriggered() {
        GameManager.Instance.Mode = GameMode.Challenge;
        DisplayScoreEvent.Invoke(new ScoreData(0));

        // TODO - Show user interface in challenge mode (time limitation)
        TimeNotificationEvent.Invoke(new TimeNotificationData(true));
        TimerActivationEvent.Invoke(new TimerData(true));
    }
}