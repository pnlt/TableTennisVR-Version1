using Dorkbots.XR.Runtime;
using UnityEngine;

public abstract class BaseScoreCalculation : MonoBehaviour
{
    [SerializeField] protected bool correctCondition;

    protected void OnEnable()
    {
        ResetConditionEvent.Subscribe(ResetCondition);
    }

    public bool GetCondition()
    {
        return correctCondition;
    }

    public void SetCondition(bool flag)
    {
        correctCondition = flag;
        if (correctCondition)
        {
            EventBus<ConditionActivatedEvent>.Raise(new ConditionActivatedEvent());
        }
    }

    protected abstract void ResetCondition();

    protected void OnDisable()
    {
        ResetConditionEvent.Unsubscribe(ResetCondition);
    }
}
