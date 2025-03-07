using System;
using _Project.Scripts.Tests.Runtime.Test;
using Dorkbots.XR.Runtime;
using UnityEngine;

#region Score Events

public struct ConditionActivatedEvent : IEvent { }

public class CheckingConditionEvent : IEvent
{
    private readonly Checkpoints checkpointsManager;
    
    public Checkpoints CheckpointsManager => checkpointsManager;

    public CheckingConditionEvent(Checkpoints checkpointsManager)
    {
        this.checkpointsManager = checkpointsManager;
    }
}

#endregion

public class ScoreManagement : MonoBehaviour
{
    [SerializeField] private int conditionThreshold;
    
    private GameManager gameManager;
    private int satisfiedConditions;    // Conditions need to be satisfied (spin wheel, right line) to score 
    private EventBinding<ConditionActivatedEvent> conditionEvents;
    private EventBinding<CheckingConditionEvent> finalScoreEvents;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        conditionEvents = new EventBinding<ConditionActivatedEvent>(ActivateCondition);
        finalScoreEvents = new EventBinding<CheckingConditionEvent>(MeetCondition);
    }

    private void OnEnable()
    {
        EventBus<ConditionActivatedEvent>.Register(conditionEvents);
        EventBus<CheckingConditionEvent>.Register(finalScoreEvents);
    }

    private void ActivateCondition()
    {
        satisfiedConditions += 1;
    }

    private void MeetCondition(CheckingConditionEvent data)
    {
        CheckConditionSatisfaction(data.CheckpointsManager);
    }

    private void CheckConditionSatisfaction(Checkpoints checkingCheckpoint)
    {
        if (satisfiedConditions >= conditionThreshold)
            ScoreSuccessfully(new SuccessfulNotification(checkingCheckpoint));     
        else
            ScoreFailed(new FailedNotification(checkingCheckpoint));
    }
    
    private void ScoreSuccessfully(Notification notification)
    {
        // Plus Score
        
        notification.ResetLine();
        ResetSatisfiedConditionNum();
    }

    /// <summary>
    /// Player failed at hitting right area on spin wheel or did not complete the line correctly
    /// </summary>
    private void ScoreFailed(Notification notification)
    {
        //UIManager.Instance.SetValueDebug("Score Failed");
        
        // Remain or decrease score
        
        notification.ResetLine();
        ResetSatisfiedConditionNum();
    }

    /// <summary>
    /// Reset the number of satisfied condition to be zero, being its initial state
    /// </summary>
    private void ResetSatisfiedConditionNum()
    {
        satisfiedConditions = 0;
    }

    private void OnDisable()
    {
        EventBus<ConditionActivatedEvent>.Deregister(conditionEvents);
        EventBus<CheckingConditionEvent>.Deregister(finalScoreEvents);
    }
}
