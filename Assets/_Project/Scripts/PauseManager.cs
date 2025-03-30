using System;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour, IPathTrigger
{
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Image image;

    private bool isPausing;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }

    private void Pause() {
        isPausing = !isPausing;
        if (isPausing)
        {
            PauseNotificationEvent.Invoke(new PauseNotificationData(true));
            image.sprite = playSprite;
            Time.timeScale = 0f;
        }
        else
        {
            PauseNotificationEvent.Invoke(new PauseNotificationData(false));
            image.sprite = pauseSprite;
            Time.timeScale = 1f;
        }
    }


    public void OnPathTriggered() {
        Pause();
    }
}