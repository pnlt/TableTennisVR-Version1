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

    [SerializeField] SoundData soundData;

    private bool _isCurrentlySwinging = false;
    private float _lastSoundPlayTime = 0f;
    private float _soundCooldown = 0.2f;
    private float _realSwingThreshold = 1.0f;
    private float _grabStabilizationTime = 0.3f;
    private float _grabStartTime = 0f;
    private bool _justGrabbed = false;

    
    private void Update()
    {
        // Only check grab state and handle sound effects in Update
        if (_grabbable != null)
        {
            if (_grabbable.SelectingPointsCount > 0)
            {
                if (!_justGrabbed)
                {
                    _justGrabbed = true;
                    _grabStartTime = Time.time;
                }
            }
            else
            {
                _justGrabbed = false;
                controllerVelocity = Vector3.zero;
            }
        }

        // Ignore velocity for a short time after grabbing
        if (_justGrabbed && Time.time - _grabStartTime < _grabStabilizationTime)
        {
            return;
        }

        // Sound logic - keep this in Update as it's not physics-related
        bool isSwinging = controllerVelocity.magnitude > _realSwingThreshold;
        if (isSwinging)
        {
            if (!_isCurrentlySwinging || (Time.time - _lastSoundPlayTime > _soundCooldown))
            {
                PlaySwingSound();
                _lastSoundPlayTime = Time.time;
            }
            _isCurrentlySwinging = true;
        }
        else
        {
            _isCurrentlySwinging = false;
        }
    }

    private void FixedUpdate()
    {
        // Handle all physics operations in FixedUpdate
        if (_grabbable != null && _grabbable.SelectingPointsCount > 0)
        {
            TrackControllerVelocity();
        }
        else
        {
            controllerVelocity = Vector3.zero;
        }
    }
    private void PlaySwingSound()
    {
        SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();

        soundBuilder
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play(soundData);
    }

    private void TrackControllerVelocity()
    {
        OVRInput.Controller activeController = OVRInput.GetActiveController();
        if (activeController == OVRInput.Controller.None)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            controllerVelocity = Vector3.zero;
            return;
        }

        controllerVelocity = OVRInput.GetLocalControllerVelocity(activeController);
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