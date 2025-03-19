using Dorkbots.XR.Runtime;
using UnityEngine;

public abstract class BaseScoreCalculation : MonoBehaviour
{
    [SerializeField] protected bool correctCondition;

    protected bool isInAction = false;

    protected void OnEnable()
    {
        ResetConditionEvent.Subscribe(ResetCondition);
    }

    public bool GetCondition()
    {
        return correctCondition;
    }

    public abstract void SetCondition(bool flag);

    protected abstract void ResetCondition();

    protected void OnDisable()
    {
        ResetConditionEvent.Unsubscribe(ResetCondition);
    }
}
