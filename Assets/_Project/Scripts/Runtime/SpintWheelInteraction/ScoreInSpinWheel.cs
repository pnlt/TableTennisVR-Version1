using System;
using UnityEngine;

public class ScoreInSpinWheel : BaseScoreCalculation
{
    public override void SetCondition(bool flag)
    {
        correctCondition = flag;
        if (!isInAction)
        {
            isInAction = true;
            if (correctCondition)
            {
                EventBus<ConditionActivatedEvent>.Raise(new ConditionActivatedEvent());
            }
        }
    }

    protected override void ResetCondition()
    {
        isInAction = false;
        if (!correctCondition)
        {
            Debug.Log("Did not hit right spin wheel area");
            return;
        }
        correctCondition = false;
    }
}

