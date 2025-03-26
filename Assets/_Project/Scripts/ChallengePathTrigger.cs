using System;
using Michsky.UI.Heat;
using UnityEngine;
using UnityEngine.UI;

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
        else
        {
            originalGradient.EffectGradient = initialGradient;
            originalGradient.Offset = initialOffset;
            originalGradient.Zoom = initialZoom;
        }
    }

    public void OnPathTriggered() {
    }
}