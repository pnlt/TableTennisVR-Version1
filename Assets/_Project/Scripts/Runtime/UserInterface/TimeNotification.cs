using System;
using TMPro;
using UnityEngine;

public class TimeNotification : MonoBehaviour
{
    [Header ("Reference components")]
    [SerializeField] private TextMeshProUGUI timeTxt;
    
    private LevelSO currentLevel;
    private float limitedTime = 90;

    private void Awake()
    {
        currentLevel = GameManager.Instance.CurrentLevel;
        limitedTime = currentLevel.respectiveChallenge.limitedTime;
    }

    private void Update()
    {
        TimePass();
    }

    private void TimePass()
    {
        if (IsOutOfTime(limitedTime))
        {
            // Trigger event
            return;
        }
        
        timeTxt.text = TimeSpan.FromSeconds(limitedTime).ToString(@"mm\:ss");
        limitedTime -= Time.deltaTime;   
    }

    private bool IsOutOfTime(float limitedTime)
    {
        if (limitedTime <= 0) return false;

        return true;
    }
}
