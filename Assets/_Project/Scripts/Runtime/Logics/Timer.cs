using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    public delegate void OnTimeOut();
    public static event OnTimeOut OnTimerEnded;
    
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
    }
    
    private void TimePass()
    {
        _isTimeOut = IsOutOfTime(limitedTime);
        if (_isTimeOut)
        {
            OnTimerEnded?.Invoke();
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
