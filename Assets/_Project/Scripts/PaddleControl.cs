
using System;
using _Project.Scripts.Runtime.Enum;
using AudioSystem;
using Dorkbots.XR.Runtime;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

    public class PaddleControl : MonoBehaviour
    {
        [SerializeField] private Grabbable _grabbable;
        [SerializeField]
        private Rigidbody _rigidbody; 

        private const float _velocityThreshold = 0.01f; 
        private const float _velocityMultiplier = 5f;  
        private Vector3 _previousVelocity;
        private Vector3 acceleration;
        private Vector3 controllerVelocity;
        
        [SerializeField] SoundData soundData;

        private bool _wasAboveThresholdLastFrame;
        private bool _wasGrabbingLastFrame;


        private void FixedUpdate()
      {
          bool isGrabbing = _grabbable != null && _grabbable.SelectingPointsCount > 0;

          if (isGrabbing)
          {
              TrackControllerVelocity();
              float speed = controllerVelocity.magnitude;
              bool isAboveThreshold = speed > _velocityThreshold;

              if (isAboveThreshold && !_wasAboveThresholdLastFrame && _wasGrabbingLastFrame)
              {
                  PlaySwingSound();
              }

              _wasAboveThresholdLastFrame = isAboveThreshold;
          }
          else
          {
              _wasAboveThresholdLastFrame = false;
          }

          _wasGrabbingLastFrame = isGrabbing;
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