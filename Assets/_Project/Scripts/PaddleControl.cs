
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

        
        private const float _swingThreshold = 1.5f;
        private float timeSinceLastSound = 0f;
        private float soundCooldown = 0.25f;  // Increased to prevent rapid sound repetition
        private bool wasSwinging = false; 

        
        private void Update()
        {
            timeSinceLastSound += Time.deltaTime;
        
            bool isSwinging = controllerVelocity.magnitude > _swingThreshold;
        
            if (isSwinging && !wasSwinging && timeSinceLastSound >= soundCooldown)
            {
                SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
            
                float swingIntensity = Mathf.Clamp01(controllerVelocity.magnitude / 10f);
            
                soundBuilder
                    .WithRandomPitch()
                    .WithPosition(transform.position)
                    .Play(soundData);
                
                timeSinceLastSound = 0f;
            }
        
            wasSwinging = isSwinging;
        }

        private void FixedUpdate()
      {
          if (_grabbable != null && _grabbable.SelectingPointsCount > 0)
          {
              TrackControllerVelocity();
          }
          else
          {
              // Reset velocity when not grabbed
              controllerVelocity = Vector3.zero;
              wasSwinging = false;  // Reset swing state when not grabbed
          }
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