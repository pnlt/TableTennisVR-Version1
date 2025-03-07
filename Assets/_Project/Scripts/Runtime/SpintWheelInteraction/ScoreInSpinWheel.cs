using System;
using UnityEngine;

public class ScoreInSpinWheel : BaseScoreCalculation
{
    private void Update()
    {
        if (correctCondition)
            TestCheckpoint.listCheckUI[1].ChangeColor(Color.green);
    }

    protected override void ResetCondition()
    {
        if (!correctCondition)
        {
            Debug.Log("Did not hit right spin wheel area");
            //UIManager.Instance.SetValueDebug("Did not hit right spin wheel area");
            return;
        }
        correctCondition = false;
    }
}

