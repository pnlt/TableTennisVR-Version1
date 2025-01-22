using _Project.Scripts.Tests.Runtime.Test;
using UnityEngine;

public class SplineCheckpoints : MonoBehaviour
{
    [SerializeField] private float countDownTime = 3;       // The max length of time of each checkpoint
    public Checkpoints checkpoints;
                                                            // allows user continuing to follow line 
    private bool _isInTurn = false;
    
    public bool IsCountDown { get; set; }
    public bool IsInTurn
    {
        get { return _isInTurn; }
        set { _isInTurn = value; }
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
        countDownTime -= Time.deltaTime;
        if (countDownTime <= 0)
        {
            // Out of time => Failed line then resetting it
            Notification failedLine = new FailedNotification(checkpoints);
            failedLine.ResetLine();
            IsCountDown = false;
            countDownTime = 3;
        }
    }
}
