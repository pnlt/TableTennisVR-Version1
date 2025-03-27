using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ModeBtnController : MonoBehaviour
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

    private void OnYesClicked()
    {
        
    }

    private void OnNoClicked()
    {
        //remain in normal mode
    }


    private void OnDisable()
    {
        yesButton.onClick.RemoveListener(OnYesClicked);
        noButton.onClick.RemoveListener(OnNoClicked);
    }
}