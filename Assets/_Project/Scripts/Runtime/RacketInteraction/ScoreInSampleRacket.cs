using System;
using System.Collections.Generic;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class ScoreInSampleRacket : BaseScoreCalculation
{
    [SerializeField] private ScoreManagement scoreManagement;
    [SerializeField] private List<IllustrativeRacket> illustratives = new List<IllustrativeRacket>();

    public void AddSampleRacket(IllustrativeRacket illustrativeRacket) {
        illustratives.Add(illustrativeRacket);
    }

    public override void SetCondition(bool flag) {
        correctCondition = flag;
        //UIManager.Instance.SetValueDebug($"{correctCondition}");

        if (scoreManagement)
            scoreManagement.CorrectPose = correctCondition;
    }

    protected override void ResetCondition() {
        ResetOriginalSampleRacket();
        if (!correctCondition)
        {
            // Notify the failure of posing sample racket

            return;
        }

        correctCondition = false;
    }

    private void ResetOriginalSampleRacket() {
        foreach (var illustrativeRacket in illustratives)
        {
            illustrativeRacket.SetMatToOrigin();
        }

        illustratives.Clear();
    }
}