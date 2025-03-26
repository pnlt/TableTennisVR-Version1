using System;
using System.Globalization;
using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using TMPro;
using UnityEngine;

public class ScoreDisplayment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private float UIScore;
    private int requiredScore;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void Start() {
        ScoreTransfer(new ScoreData(gameManager.CurrentLevel.practiceScore));
    }

    private void OnEnable() {
        DisplayScoreEvent.Subscribe(ScoreTransfer);
    }

    private void ScoreTransfer(ScoreData scoreData) {
        ModeFilter();

        // Using PlayerPref to store the real value that displayed on the UI
        UIScore = scoreData.Score;
        scoreTxt.text = UIScore.ToString(CultureInfo.CurrentUICulture) + $"/{requiredScore}";
    }

    private void ModeFilter() {
        if (gameManager.Mode == GameMode.Practice)
            requiredScore = gameManager.CurrentLevel.requiredScore;
        else if (gameManager.Mode == GameMode.Challenge)
            requiredScore = gameManager.CurrentLevel.respectiveChallenge.requiredScore;
    }

    private void OnDisable() {
        DisplayScoreEvent.Unsubscribe(ScoreTransfer);
    }
}