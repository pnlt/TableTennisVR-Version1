using System;
using UnityEngine;

public class ScoreInSpinWheel : BaseScoreCalculation
{
    public override void SetCondition(bool flag)
    {
        correctCondition = flag;
        if (correctCondition)
        {
            EventBus<ConditionActivatedEvent>.Raise(new ConditionActivatedEvent());
        }
    }

    protected override void ResetCondition()
    {
        if (!correctCondition)
        {
            Debug.Log("Did not hit right spin wheel area");
            return;
        }
        correctCondition = false;
    }
}

