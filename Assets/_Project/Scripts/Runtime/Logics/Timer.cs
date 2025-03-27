using System;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public delegate void OnTimeOut();

    public static event OnTimeOut OnTimerEnded;

    private LevelSO currentLevel;
    private float limitedTime;
    private bool _isTimeOut;

    private void Awake() {
        currentLevel = GameManager.Instance.CurrentLevel;
        limitedTime = currentLevel.respectiveChallenge.limitedTime;
    }

    private void OnEnable() {
        TimerActivationEvent.Unsubscribe(ActivateTimer);
        TimerActivationEvent.Subscribe(ActivateTimer);

        DisplayTimerEvent.Invoke(new DisplayTimerData(limitedTime));
    }

    private void Start() {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private void ActivateTimer(TimerData data) {
        gameObject.SetActive(data.Flag);
    }

    private void Update() {
        TimePass();
    }

    private void TimePass() {
        _isTimeOut = IsOutOfTime(limitedTime);
        if (_isTimeOut)
        {
            // TODO - Display Notification and reset the challenge state
            OnTimerEnded?.Invoke();
            
            ResetTimer();
            
            gameObject.SetActive(false);
            return;
        }

        DisplayTimerEvent.Invoke(new DisplayTimerData(limitedTime));
        limitedTime -= Time.deltaTime;
    }

    private void ResetTimer()
    {
        _isTimeOut = false;
        limitedTime = currentLevel.respectiveChallenge.limitedTime;
    }

    private bool IsOutOfTime(float limitedTime) {
        if (limitedTime <= 0) return true;

        return false;
    }
}