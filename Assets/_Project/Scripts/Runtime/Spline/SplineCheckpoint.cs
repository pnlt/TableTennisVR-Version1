using UnityEngine;

public class SplineCheckpoint : MonoBehaviour
{
    [SerializeField] private float countDownTime;       // The max length of time of each checkpoint
                                                            // allows user continuing to follow line 
    public Checkpoints checkpoints;
    public bool IsCountDown { get; set; }
    public bool IsInTurn
    {
        get => _isInTurn;
        set => _isInTurn = value;
    }
    
    private bool _isInTurn;
    private float tempCountdownTime;

    private void Start()
    {
        _isInTurn = true;
        tempCountdownTime = countDownTime;
    }

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
        tempCountdownTime -= Time.deltaTime;
        if (tempCountdownTime <= 0)
        {
            ResetCountDown();
            // Out of time => Failed line then resetting it
            EventBus<CheckingConditionEvent>.Raise(new CheckingConditionEvent(checkpoints));
        }
    }

    public void ResetCountDown()
    {
        IsCountDown = false;
        tempCountdownTime = countDownTime;
    }
}
