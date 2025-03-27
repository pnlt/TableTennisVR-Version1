using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DecisionBtnController : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void OnEnable()
    {
        if (yesButton && noButton)
        {
            yesButton.onClick.AddListener(OnYesClicked);
            noButton.onClick.AddListener(OnNoClicked);
        }
    }

    /// <summary>
    /// Player agrees with the option of playing challenge again
    /// </summary>
    private void OnYesClicked()
    {
        FailedChallengeNotification.Invoke(new FailedChallengeNotificationData(false));
        
        // TODO - Enable score system so that player can score
        EventBus<ScoreActivationEvent>.Raise(new ScoreActivationEvent(true));
        
        DisplayScoreEvent.Invoke(new ScoreData(0));
        
        TimerActivationEvent.Invoke(new TimerData(true));
    }

    /// <summary>
    /// Player wants to go back practice to train more
    /// </summary>
    private void OnNoClicked()
    {
        FailedChallengeNotification.Invoke(new FailedChallengeNotificationData(false));
        
        // TODO - Enable score system so that player can score
        EventBus<ScoreActivationEvent>.Raise(new ScoreActivationEvent(true));
        
        TimeNotificationEvent.Invoke(new TimeNotificationData(false));
        
        // TODO - Go back to practice mode
        GameManager.Instance.Mode = GameMode.Practice;
        
        DisplayScoreEvent.Invoke(new ScoreData(GameManager.Instance.CurrentLevel.practiceScore));
    }


    private void OnDisable()
    {
        yesButton.onClick.RemoveListener(OnYesClicked);
        noButton.onClick.RemoveListener(OnNoClicked);
    }
}