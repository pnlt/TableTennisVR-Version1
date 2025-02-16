
using Oculus.Interaction.HandGrab;
using UnityEngine;

    public class PaddleControl : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody; // Rigidbody of the paddle

        private const float _velocityThreshold = 0.01f; // Minimum velocity to consider
        private const float _velocityMultiplier = 5f;  // Multiplier to amplify the velocity
        private Vector3 _previousVelocity;
        private Vector3 acceleration;

        private void FixedUpdate()
        {
            TrackControllerVelocity();
        }

        private void TrackControllerVelocity()
        {
            // Declare the controller that will be actively used (left or right)
            OVRInput.Controller activeController = OVRInput.Controller.None;

            // Check if the left or right controller is connected
            if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
            {
                activeController = OVRInput.Controller.LTouch;
            }
            else if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
            {
                activeController = OVRInput.Controller.RTouch;
            }

            // If a controller is connected
            if (activeController != OVRInput.Controller.None)
            {
                // Get the velocity of the active controller
                Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(activeController);

                // Apply a multiplier to increase the effect of the velocity if it's too low
                controllerVelocity *= _velocityMultiplier;

                // Only apply the velocity if it's above the threshold
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
            else
            {
                // Reset the velocity if no controller is connected
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