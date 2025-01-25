using System;
using UnityEngine;

namespace _Project.Scripts.Tests.Runtime.RacketInteraction
{
    public class PhysicalWheel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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