using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    public delegate void CheckTime(bool isTimeOut);
    public static event CheckTime OnTimeOut;
    
    private LevelSO currentLevel;
    private float limitedTime = 90;
    private bool _isTimeOut;

    private void Awake()
    {
        currentLevel = GameManager.Instance.CurrentLevel;
        limitedTime = currentLevel.respectiveChallenge.limitedTime;
    }

    private void OnEnable()
    {
        DisplayTimerEvent.Invoke(new DisplayTimerData(limitedTime));
    }

    private void Update()
    {
        TimePass();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnTimeOut?.Invoke(true);
            Debug.Log("<color>Event</color>");
        }
    }
    
    private void TimePass()
    {
        _isTimeOut = IsOutOfTime(limitedTime);
        if (_isTimeOut)
        {
            OnTimeOut?.Invoke(_isTimeOut);
            return;
        }
        
        DisplayTimerEvent.Invoke(new DisplayTimerData(limitedTime));
        limitedTime -= Time.deltaTime;   
    }

    private bool IsOutOfTime(float limitedTime)
    {
        if (limitedTime <= 0) return true;

        return false;
    }
}
