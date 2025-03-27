using System;
using _Project.Scripts.Runtime.Enum;
using _Project.Scripts.Runtime.Interfaces;
using _Project.Scripts.Tests.Runtime.Test;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public struct ConditionActivatedEvent : IEvent
{
}

public class CheckingConditionEvent : IEvent
{
    private readonly Checkpoints checkpointsManager;

    public Checkpoints CheckpointsManager => checkpointsManager;

    public CheckingConditionEvent(Checkpoints checkpointsManager)
    {
        this.checkpointsManager = checkpointsManager;
    }
}

public struct ScoreActivationEvent : IEvent
{
    private readonly bool flag;
    public bool Flag => flag;

    public ScoreActivationEvent(bool flag)
    {
        this.flag = flag;
    }
}

public class ScoreManagement : MonoBehaviour
{
    [SerializeField] private int conditionThreshold;

    private LevelSO presentLevel;
    private GameManager gameManager;
    private int satisfiedConditions; // Conditions need to be satisfied (spin wheel, right line) to score 
    private bool correctPose; // Needed condition to get score (have correction swinging pose)
    private EventBinding<ConditionActivatedEvent> conditionEvents;
    private EventBinding<CheckingConditionEvent> finalScoreEvents;
    private EventBinding<ScoreActivationEvent> scoreActivationEvent;

    public bool CorrectPose
    {
        get => correctPose;
        set => correctPose = value;
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        conditionEvents = new EventBinding<ConditionActivatedEvent>(ActivateCondition);
        finalScoreEvents = new EventBinding<CheckingConditionEvent>(MeetCondition);
        scoreActivationEvent = new EventBinding<ScoreActivationEvent>(ScoreSystemActivation);
    }

    private void Start()
    {
        presentLevel = gameManager.CurrentLevel;
    }

    private void OnEnable()
    {
        EventBus<ConditionActivatedEvent>.Register(conditionEvents);
        EventBus<CheckingConditionEvent>.Register(finalScoreEvents);
        EventBus<ScoreActivationEvent>.Register(scoreActivationEvent);
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            if (gameManager.Mode == GameMode.Practice)
                presentLevel.UpdateScore(gameManager);
            else if (gameManager.Mode == GameMode.Challenge)
                presentLevel.ChallengeUpdate();
        }
    }

    private void ScoreSystemActivation(ScoreActivationEvent scoreActivationEvent)
    {
        gameObject.SetActive(scoreActivationEvent.Flag);
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
        UIManager.Instance.SetValueDebug($"Correct Pose: {correctPose} + {satisfiedConditions}");
        if (satisfiedConditions >= 2 && correctPose)
            ScoreSuccessfully(new SuccessfulNotification(checkingCheckpoint), presentLevel);
        else
            ScoreFailed(new FailedNotification(checkingCheckpoint), presentLevel);

        ResetSatisfiedConditionNum();
    }

    private void ScoreSuccessfully(Notification successfulNotification, IScoreIncrease level)
    {
        // Plus Score
        //UIManager.Instance.SetValueDebug("Success");
        if (gameManager.Mode == GameMode.Practice)
            level.UpdateScore(gameManager);
        else if (gameManager.Mode == GameMode.Challenge) // In challenge mode
        {
            level.ChallengeUpdate();
        }

        successfulNotification.ResetLine();
    }

    /// <summary>
    /// Player failed at hitting right area on spin wheel or did not complete the line correctly
    /// </summary>
    private void ScoreFailed(Notification failedNotification, IScoreDecrease level)
    {
        //UIManager.Instance.SetValueDebug("Failed");
        // Remain or decrease score
        if (gameManager.Mode == GameMode.Challenge)
            level.ScoreDecrease(satisfiedConditions, correctPose);

        failedNotification.ResetLine();
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
        EventBus<ScoreActivationEvent>.Deregister(scoreActivationEvent);
    }
}