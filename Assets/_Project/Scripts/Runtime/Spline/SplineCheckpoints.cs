using _Project.Scripts.Tests.Runtime.Test;
using UnityEngine;

public class SplineCheckpoints : MonoBehaviour
{
    [SerializeField] private float countDownTime = 3;       // The max length of time of each checkpoint
                                                            // allows user continuing to follow line 
    public Checkpoints checkpoints;
    public bool IsCountDown { get; set; }
    public bool IsInTurn
    {
        get => _isInTurn;
        set => _isInTurn = value;
    }
    
    private bool _isInTurn;

    private void Update()
    {
        if (IsCountDown)
        {
            CountingDown();
        }
    }

    /// <summary>
    /// What events happen when each checkpoint is out of allowed time
    /// </summary>
    private void CountingDown()
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime <= 0)
        {
            ResetCountDown();
            // Out of time => Failed line then resetting it
            EventBus<CheckingConditionEvent>.Raise(new CheckingConditionEvent(checkpoints));
        }
    }

    public void ResetCountDown()
    {
        IsCountDown = false;
        countDownTime = 3;
    }
}
