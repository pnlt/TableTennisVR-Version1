using System;
using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ModeBtnController : MonoBehaviour
{
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;

    private void OnEnable()
    {
        if (yesBtn && noBtn)
        {
            yesBtn.onClick.AddListener(OnYesClicked);
            noBtn.onClick.AddListener(OnNoClicked);
        }
    }

    private void OnYesClicked()
    {
        GameManager.Instance.Mode = GameMode.Challenge;
        DisplayScoreEvent.Invoke(new ScoreData(0));
        // Show user interface in challenge mode (time limitation)

        // Play some anim
        ModeAlterationNotificationEvent.Invoke(new ModeNotificationData(false));
    }

    private void OnNoClicked()
    {
        //remain in normal mode

        // Play some anim
        ModeAlterationNotificationEvent.Invoke(new ModeNotificationData(false));
    }


    private void OnDisable()
    {
        yesBtn.onClick.RemoveListener(OnYesClicked);
        noBtn.onClick.RemoveListener(OnNoClicked);
    }
}