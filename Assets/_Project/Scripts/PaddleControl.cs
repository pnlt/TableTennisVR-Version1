using AudioSystem;
using Oculus.Interaction;
using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private Rigidbody _rigidbody;

    private const float _velocityThreshold = 0.01f;
    private const float _velocityMultiplier = 5f;
    private Vector3 _previousVelocity;
    private Vector3 acceleration;
    private Vector3 controllerVelocity;

    private void FixedUpdate()
    {
        if (_grabbable != null && _grabbable.SelectingPointsCount > 0)
        {
            TrackControllerVelocity();
        }
    }

    private void TrackControllerVelocity()
    {
        controllerVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        controllerVelocity *= _velocityMultiplier;


        if (controllerVelocity.magnitude > _velocityThreshold)
        {
            _rigidbody.linearVelocity = controllerVelocity;
            AccelerationCalculation(_rigidbody.linearVelocity);
        }
        else
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }

    public Vector3 ForceApplied()
    {
        return acceleration * _rigidbody.mass;
    }

    private void AccelerationCalculation(Vector3 paddleVelocity)
    {
        var deltaVelocity = paddleVelocity - _previousVelocity;
        acceleration = deltaVelocity / Time.fixedUnscaledDeltaTime;

        _previousVelocity = paddleVelocity;
    }
}