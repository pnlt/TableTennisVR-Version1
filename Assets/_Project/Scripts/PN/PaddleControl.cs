using AudioSystem;
using Oculus.Interaction;
using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private Rigidbody _rigidbody;

    private const float _velocityThreshold = 0.01f;
    private const float _velocityMultiplier = 2f;
    private Vector3 _previousVelocity;
    private Vector3 acceleration;
    private Vector3 controllerVelocity;

    [SerializeField] SoundData soundData;

    // Sound and swing detection variables
    private float _swingThreshold = 7.0f;
    private bool _isCurrentlySwinging = false;
    private float _lastSoundPlayTime = 0f;
    private float _soundCooldown = 0.3f;
    private bool _wasGrabbed = false;

    private void FixedUpdate() {
        if (_grabbable != null && _grabbable.SelectingPointsCount > 0)
        {
            TrackControllerVelocity();

            // Detect if this is a new grab
            if (!_wasGrabbed)
            {
                _wasGrabbed = true;
                _isCurrentlySwinging = false;
            }

            // Check for swing with higher threshold
            bool isSwinging = controllerVelocity.magnitude > _swingThreshold;

            // Handle swing sound logic
            if (isSwinging && (!_isCurrentlySwinging || Time.time - _lastSoundPlayTime > _soundCooldown))
            {
                PlaySwingSound();
                _lastSoundPlayTime = Time.time;
                _isCurrentlySwinging = true;
            }
            else if (!isSwinging)
            {
                _isCurrentlySwinging = false;
            }
        }
        else
        {
            _wasGrabbed = false;
            _isCurrentlySwinging = false;
            controllerVelocity = Vector3.zero;
        }
    }

    private void PlaySwingSound() {
        SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();

        soundBuilder
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play(soundData);
    }

    private void TrackControllerVelocity() {
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

    public Vector3 ForceApplied() {
        return acceleration * _rigidbody.mass;
    }

    private void AccelerationCalculation(Vector3 paddleVelocity) {
        var deltaVelocity = paddleVelocity - _previousVelocity;
        acceleration = deltaVelocity / Time.fixedUnscaledDeltaTime;

        _previousVelocity = paddleVelocity;
    }
}