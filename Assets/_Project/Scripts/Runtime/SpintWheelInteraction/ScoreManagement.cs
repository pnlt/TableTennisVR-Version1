using System;
using _Project.Scripts.Runtime.Enum;
using _Project.Scripts.Runtime.Interfaces;
using _Project.Scripts.Tests.Runtime.Test;
using UnityEngine;

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

public class ScoreManagement : MonoBehaviour
{
    [SerializeField] private int conditionThreshold;
    public LevelSO presentLevel;
    
    private GameManager gameManager;
    private int satisfiedConditions;    // Conditions need to be satisfied (spin wheel, right line) to score 
    private bool correctPose;      // Needed condition to get score (have correction swinging pose)
    private EventBinding<ConditionActivatedEvent> conditionEvents;
    private EventBinding<CheckingConditionEvent> finalScoreEvents;
    
    public bool CorrectPose { get => correctPose; set => correctPose = value; }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        conditionEvents = new EventBinding<ConditionActivatedEvent>(ActivateCondition);
        finalScoreEvents = new EventBinding<CheckingConditionEvent>(MeetCondition);
    }

    private void Start()
    {
        presentLevel = gameManager.CurrentLevel;
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
        presentLevel = gameManager.CurrentLevel;
        if (satisfiedConditions >= conditionThreshold && correctPose)
            ScoreSuccessfully(new SuccessfulNotification(checkingCheckpoint), presentLevel);     
        else
            ScoreFailed(new FailedNotification(checkingCheckpoint), presentLevel);
    }
    
    private void ScoreSuccessfully(Notification successfulNotification, IScoreIncrease presentLevel)
    {
        // Plus Score
        UIManager.Instance.SetValueDebug("Success");
        if (gameManager.Mode == GameMode.Normal)
            presentLevel.UpdateScore(gameManager);
        else if (gameManager.Mode == GameMode.Challenge)    // In challenge mode
        {
            presentLevel.ChallengeUpdate(gameManager);
        }
        
        successfulNotification.ResetLine();
        ResetSatisfiedConditionNum();
    }

    /// <summary>
    /// Player failed at hitting right area on spin wheel or did not complete the line correctly
    /// </summary>
    private void ScoreFailed(Notification failedNotification, IScoreDecrease presentLevel)
    {
        UIManager.Instance.SetValueDebug("Failed");
        // Remain or decrease score
        if (gameManager.Mode == GameMode.Normal)
            presentLevel.ScoreDecrease(gameManager, satisfiedConditions,  correctPose);
        
        failedNotification.ResetLine();
        ResetSatisfiedConditionNum();
    }

    /// <summary>
    /// Reset the number of satisfied condition to be zero, being its initial state
    /// </summary>
    private void ResetSatisfiedConditionNum()
    {
        satisfiedConditions = 0;
        correctPose = false;
    }

    private void OnDisable()
    {
        EventBus<ConditionActivatedEvent>.Deregister(conditionEvents);
        EventBus<CheckingConditionEvent>.Deregister(finalScoreEvents);
    }
}
