using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

namespace Dorkbots.XR.Runtime.RacketInteraction
{
    public class CollisionDetection : MonoBehaviour
    {
        [Header("Reference components")] [SerializeField]
        private Transform playerRacket;

        [SerializeField] private LayerMask racketLayer;
        [SerializeField] private MeshFilter meshShape;

        [Header("Materials")] [SerializeField] private Material correctMat;
        [SerializeField] private Material incorrectMat;

        [Header("Threshold")] [SerializeField] private float distanceThreshold;

        private IllustrativeRacket racket;
        private bool isOutOfRange;
        private int collisionLayerValue;

        public bool OutOfRange
        {
            get => isOutOfRange;
            set => isOutOfRange = value;
        }

        private void Awake() {
            ReferenceComponents();
        }

        private void ReferenceComponents() {
            racket = GetComponentInParent<IllustrativeRacket>();
        }

        private void OnTriggerEnter(Collider other) {
            collisionLayerValue = 1 << other.gameObject.layer;
        }

        private void OnTriggerStay(Collider other) {
            if (collisionLayerValue != racketLayer)
                return;

            if (isOutOfRange)
            {
                PoseCorrectionSignal();
                return;
            }

            PoseCorrectionSignal();
        }

        private float currentAlignmentScore;
        private float smoothingFactor = 0.3f; // Adjust for more/less smoothing

        private void PoseCorrectionSignal() {
            // Call racket's method to check alignment, the tolerance is used inside the racket class
            float alignmentScore = racket.CalculateAlignmentScore(transform.parent, playerRacket, meshShape);

            // Smooth the score over time to prevent flickering
            currentAlignmentScore = Mathf.Lerp(currentAlignmentScore, alignmentScore, smoothingFactor);

            // Use smoothed score for feedback
            if (currentAlignmentScore > 0.65f)
            {
                racket.ChangeMaterial(correctMat);
                racket.ConditionValidation(true);
            }
            else
            {
                racket.ChangeMaterial(incorrectMat);
                racket.ConditionValidation(false);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (collisionLayerValue == racketLayer)
                ResetState();
        }

        private void ResetState() {
            isOutOfRange = false;
        }
    }
}