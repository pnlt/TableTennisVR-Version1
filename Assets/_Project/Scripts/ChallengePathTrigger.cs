using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using Michsky.UI.Heat;
using UnityEngine;

public class ChallengePathTrigger : MonoBehaviour, IPathTrigger
{
    [SerializeField] private bool isEnabled = false;
    [SerializeField] private UIGradient sampleGradient;

    private UIGradient originalGradient;
    private Gradient initialGradient;
    private float initialOffset;
    private float initialZoom;

    private void Awake() {
        originalGradient = GetComponent<UIGradient>();
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
            if (!originalGradient)
                originalGradient = GetComponent<UIGradient>();

            isEnabled = value;
            // Update the visual state when enabled/disabled
            SetBtnColor(isEnabled);
            if (originalGradient != null)
            {
                originalGradient.enabled = false;
                originalGradient.enabled = true;
            }
        }
    }

    private void SetBtnColor(bool isEnabled) {
        if (isEnabled)
        {
            originalGradient.EffectGradient = sampleGradient.EffectGradient;
            originalGradient.Offset = sampleGradient.Offset;
            originalGradient.Zoom = sampleGradient.Zoom;
        }
        else
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