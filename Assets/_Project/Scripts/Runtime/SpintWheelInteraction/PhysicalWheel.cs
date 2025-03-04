using System;
using UnityEngine;

namespace _Project.Scripts.Tests.Runtime.RacketInteraction
{
    public class PhysicalWheel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        
        private float rotationMultiplier = 1f;
        private Collider childCollider;
        private Collider parentCollider;
        
        // Tweak these for your desired spin effect
        [Header("Spin Audio Settings")]
        [SerializeField] private float maxSpinSpeed = 20f;
        [SerializeField] private float minPitch = 0.5f;
        [SerializeField] private float maxPitch = 2.0f;
        [SerializeField] private float minVolume = 0f;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float spinThreshold = 0.1f; // If speed < this, consider it "stopped"

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            childCollider = GetComponentInChildren<Collider>();
            parentCollider = GetComponent<Collider>();
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
                // If not already playing, start it
                if (!AudioManager.instance.IsPlaying("SpinWheelLoop"))
                {
                    AudioManager.instance.Play("SpinWheelLoop");
                }

                // 3) Calculate normalized speed [0..1]
                float t = spinSpeed / maxSpinSpeed;
                t = Mathf.Clamp01(t);

                // 4) Interpolate pitch & volume
                float newPitch = Mathf.Lerp(minPitch, maxPitch, t);
                float newVolume = Mathf.Lerp(minVolume, maxVolume, t);

                // 5) Update the sound in real time
                AudioManager.instance.UpdateSound("SpinWheelLoop", newVolume, newPitch);
            }
            else
            {
                // If spinning slower than threshold, stop the loop
                if (AudioManager.instance.IsPlaying("SpinWheelLoop"))
                {
                    AudioManager.instance.Stop("SpinWheelLoop");
                }
            }
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