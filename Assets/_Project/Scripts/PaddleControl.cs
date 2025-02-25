
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

    public class PaddleControl : MonoBehaviour
    {
        [SerializeField] private Grabbable _grabbable;
        [SerializeField]
        private Rigidbody _rigidbody; // Rigidbody of the paddle

        private const float _velocityThreshold = 0.01f; // Minimum velocity to consider
        private const float _velocityMultiplier = 5f;  // Multiplier to amplify the velocity
        private Vector3 _previousVelocity;
        private Vector3 acceleration;
        

      private void FixedUpdate()
      {
                 if (_grabbable != null && _grabbable.SelectingPointsCount > 0)
                 {
                     OVRInput.Controller activeController = OVRInput.GetActiveController();
                     if (activeController != OVRInput.Controller.None)
                     {
                         Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(activeController);
                         controllerVelocity *= _velocityMultiplier;
         
                         if (controllerVelocity.magnitude > _velocityThreshold)
                         {
                             AccelerationCalculation(controllerVelocity);
                         }
                         
                     }
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