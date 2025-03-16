using System;
using UnityEngine;

public class ScoreInSpinWheel : BaseScoreCalculation
{
    private void Update()
    {
        if (correctCondition)
            TestCheckpoint.listCheckUI[1].ChangeColor(Color.green);
    }

    public override void SetCondition(bool flag)
    {
        correctCondition = flag;
        if (correctCondition)
        {
            UIManager.Instance.SetValueDebug("Spline");
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

