using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreDisplayment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private float UIScore;

    private void ScoreTransfer(float score)
    {
        // Using PlayerPref to store the real value that displayed on the UI
        UIScore = score;
        scoreTxt.text = UIScore.ToString(CultureInfo.CurrentUICulture);
    }
}
