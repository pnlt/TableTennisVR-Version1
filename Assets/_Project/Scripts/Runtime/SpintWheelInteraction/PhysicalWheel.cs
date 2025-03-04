using System;
using AudioSystem;
using UnityEngine;

namespace _Project.Scripts.Tests.Runtime.RacketInteraction
{
    public class PhysicalWheel : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Rigidbody _rigidbody;

        private float rotationMultiplier = 1f;
        private Collider childCollider;
        private Collider parentCollider;

        // Tweak these for your desired spin effect
        [Header("Spin Audio Settings")] [SerializeField]
        private float maxSpinSpeed = 20f;

        [SerializeField] private float minPitch = 0.5f;
        [SerializeField] private float maxPitch = 2.0f;
        [SerializeField] private float minVolume = 0f;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float spinThreshold = 0.01f; // If speed < this, consider it "stopped"

        // Sound system references
        [SerializeField] private SoundData spinWheelLoopSound;
        private SoundEmitter currentSpinEmitter;
        private SoundBuilder soundBuilder;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            childCollider = GetComponentInChildren<Collider>();
            parentCollider = GetComponent<Collider>();

            // Cache the sound builder for better performance
            soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        }

        private void Start()
        {
            Physics.IgnoreCollision(parentCollider, childCollider);
        }

        private void Update()
        {
            // 1) Get how fast the wheel is spinning
            float spinSpeed = _rigidbody.angularVelocity.magnitude;

            // 2) Decide if we should be playing the loop
            if (spinSpeed > spinThreshold)
            {
                // 3) Calculate normalized speed [0..1]
                float t = spinSpeed / maxSpinSpeed;
                t = Mathf.Clamp01(t);

                // 4) Interpolate pitch & volume using logarithmic utilities
                float normalizedVolume = t.ToLogarithmicFraction();
                float newVolume = Mathf.Lerp(minVolume, maxVolume, normalizedVolume);

                float normalizedPitch = t.ToLogarithmicFraction();
                float newPitch = Mathf.Lerp(minPitch, maxPitch, normalizedPitch);

                // If not already playing, start it
                if (currentSpinEmitter == null)
                {
                    // Create a modified sound data for this particular instance
                    SoundData modifiedSound = CreateModifiedSoundData(spinWheelLoopSound, newVolume, newPitch);

                    // Create and play the spinning sound at this position
                    soundBuilder
                        .WithPosition(transform.position)
                        .Play(modifiedSound);

                    // Since we don't have a direct way to get the emitter, we'll need to 
                    // modify parameters on each update by stopping and starting with new values
                }
                else
                {
                    // Stop the current sound and restart with new parameters
                    currentSpinEmitter.Stop();
                    currentSpinEmitter = null;

                    // Create a modified sound with updated parameters
                    SoundData modifiedSound = CreateModifiedSoundData(spinWheelLoopSound, newVolume, newPitch);

                    // Play the updated sound
                    soundBuilder
                        .WithPosition(transform.position)
                        .Play(modifiedSound);
                }
            }
            else
            {
                // If spinning slower than threshold, stop the loop
                if (currentSpinEmitter != null)
                {
                    currentSpinEmitter.Stop();
                    currentSpinEmitter = null;
                }
            }
        }
        // Create a new SoundData with modified volume and pitch
        private SoundData CreateModifiedSoundData(SoundData original, float volume, float pitch)
        {
            // Create a copy of the original sound data
            SoundData modified = new SoundData();
    
            // Copy all properties
            modified.clip = original.clip;
            modified.mixerGroup = original.mixerGroup;
            modified.loop = true; // Force looping for continuous sound
            modified.playOnAwake = original.playOnAwake;
            modified.frequentSound = original.frequentSound;
            modified.mute = original.mute;
            modified.bypassEffects = original.bypassEffects;
            modified.bypassListenerEffects = original.bypassListenerEffects;
            modified.bypassReverbZones = original.bypassReverbZones;
            modified.priority = original.priority;
    
            // Apply our new volume and pitch
            modified.volume = volume;
            modified.pitch = pitch;
    
            // Copy remaining properties
            modified.panStereo = original.panStereo;
            modified.spatialBlend = original.spatialBlend;
            modified.reverbZoneMix = original.reverbZoneMix;
            modified.dopplerLevel = original.dopplerLevel;
            modified.spread = original.spread;
            modified.minDistance = original.minDistance;
            modified.maxDistance = original.maxDistance;
            modified.ignoreListenerVolume = original.ignoreListenerVolume;
            modified.ignoreListenerPause = original.ignoreListenerPause;
            modified.rolloffMode = original.rolloffMode;
    
            return modified;
        }

        /// <summary>
        /// This function will calculate the rotational motion based on the built-in physics in Unity - Rigidbody
        /// </summary>
        /// <param name="velocity">Velocity of swinging racket by player</param>
        /// <param name="touchedPoint">The position where the force impacts on => collision position</param>
        public void RbRotationalMotion(Vector3 velocity, Vector3 touchedPoint)
        {
            //The force of impacting object
            var force = AngularVelocityToImpulse(velocity, touchedPoint);

            // Add Force at specific position. Unity has a built-in function to
            // determine for us the angular acceleration from the torque calculated from given force
            _rigidbody.AddTorque(force, ForceMode.Force);

            // Calculate the angular velocity to use another built-in function in Unity to 
            // adjust the rational rotational speed
            //_rigidbody.angularVelocity = Vector3.zero;
        }

        public void ClaudeRotation(Vector3 force, Vector3 touchedPoint)
        {
            Vector3 directionFromCenter = (touchedPoint - _rigidbody.worldCenterOfMass).normalized;

            // Calculate torque direction using cross product
            float torqueDirection = Mathf.Sign(Vector3.Cross(directionFromCenter, force.normalized).z);

            // Calculate torque magnitude based on force magnitude
            float torqueMagnitude = force.magnitude * rotationMultiplier;

            // Apply torque around the Z axis
            Vector3 torque = Vector3.forward * torqueMagnitude * torqueDirection;
            _rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        #region AngularAcceleration

        /// <summary>
        ///  Calculate the torque impacted by a given force - MANUALLY
        /// </summary>
        /// <param name="force">The force of impacting object</param>
        /// <param name="position">The position where the force impacts on => collision position</param>
        /// <param name="mode">What force mode in Rigidbody</param>
        /// <returns></returns>
        public Vector3 ForceToTorque(Vector3 force, Vector3 position, ForceMode mode)
        {
            Vector3 torque = Vector3.Cross(position - _rigidbody.worldCenterOfMass, force);

            return torque;
        }

        private void ToDeltaTorque(Vector3 torque, ForceMode mode)
        {
            bool useMass = mode == ForceMode.Force || mode == ForceMode.Impulse;
            if (useMass)
                ApplyInertiaTensor();
        }

        private void ApplyInertiaTensor()
        {
        }

        #endregion

        #region AngularVelocity

        /// <summary>
        /// The detail of calculating the force - angular velocity to determine the rotational speed
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="position"></param>
        /// <returns>angular velocity vector</returns>
        private Vector3 AngularVelocityToImpulse(Vector3 vel, Vector3 position)
        {
            Vector3 R = position - _rigidbody.worldCenterOfMass;
            Vector3 Q = MultiplyByInertiaTensor(vel);

            if (Mathf.Abs(Vector3.Dot(R, Q)) > 1e-5)
            {
                return new Vector3();
            }

            return 0.5f * Vector3.Cross(Q, R) / R.sqrMagnitude;
        }

        private Vector3 MultiplyByInertiaTensor(Vector3 v)
        {
            return _rigidbody.rotation * Mul(Quaternion.Inverse(_rigidbody.rotation) * v, _rigidbody.inertiaTensor);
        }

        #endregion

        #region Utilities Function

        /// <summary>
        /// Division of two vectors
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        private Vector3 Div(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiplication of two vectors
        /// </summary>
        private Vector3 Mul(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        #endregion
    }
}