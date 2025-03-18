using Dorkbots.XR.Runtime;
using UnityEngine;

public class Timer : MonoBehaviour
{
    
    private LevelSO currentLevel;
    private float limitedTime = 90;

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
        if (IsOutOfTime(limitedTime))
        {
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
