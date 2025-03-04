using System;
using Dorkbots.XR.Runtime;
using UnityEngine;

#region Score Events

public struct ConditionActivatedEvent : IEvent { }
public struct FinalScoreEvent : IEvent { }

#endregion

public class ScoreManagement : MonoBehaviour
{
    [SerializeField] private int conditionThreshold;
    private GameManager gameManager;
    private int satisfiedConditions;    // Conditions need to be satisfied (spin wheel, right line) to score 
    private EventBinding<ConditionActivatedEvent> conditionEvents;
    private EventBinding<FinalScoreEvent> finalScoreEvents;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        conditionEvents = new EventBinding<ConditionActivatedEvent>(ActivateCondition);
        finalScoreEvents = new EventBinding<FinalScoreEvent>(MeetCondition);
    }

    private void OnEnable()
    {
        EventBus<ConditionActivatedEvent>.Register(conditionEvents);
        EventBus<FinalScoreEvent>.Register(finalScoreEvents);
    }

    private void ActivateCondition()
    {
        satisfiedConditions += 1;
    }

    private void MeetCondition()
    {
        CheckConditionSatisfaction();
    }

    private void CheckConditionSatisfaction()
    {
        if (satisfiedConditions >= conditionThreshold)
            ScoreSuccessfully();     
        else
            ScoreFailed();
        
        ResetConditionEvent.Invoke();
    }
    
    private void ScoreSuccessfully()
    {
        // Plus Score
        gameManager.PlayerScore += 1;
        UIManager.Instance.SetValueDebug(gameManager.PlayerScore.ToString() + " scored");
        
        // Play successful sound
        
        ResetSatisfiedConditionNum();
    }

    /// <summary>
    /// Player failed at hitting right area on spin wheel or did not complete the line correctly
    /// </summary>
    private void ScoreFailed()
    {
        UIManager.Instance.SetValueDebug("Score Failed");
        // Play failed Sound
        
        // Remain or decrease score
        
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
        EventBus<FinalScoreEvent>.Deregister(finalScoreEvents);
    }
}
