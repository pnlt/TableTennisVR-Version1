using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

namespace Dorkbots.XR.Runtime.RacketInteraction
{
    public class CollisionDetection : MonoBehaviour
    {
        [Header("Reference components")] [SerializeField]
        private Transform playerRacket;

        public Collider racketCollider;
        public LayerMask racketLayer;
        [SerializeField] private MeshFilter meshShape;

        [Header("Materials")] [SerializeField] private Material correctMat;
        [SerializeField] private Material incorrectMat;

        [Header("Threshold")] [SerializeField] private float distanceThreshold;

        private IllustrativeRacket racket;
        private Collider sampleRacketCollider;
        private bool isOutOfRange;

        private bool isCorrectPose;
        private bool inCenter;
        
        public bool OutOfRange { get => isOutOfRange; set => isOutOfRange = value; }

        private void Awake() {
            ReferenceComponents();
        }

        private void ReferenceComponents() {
            racket = GetComponentInParent<IllustrativeRacket>();
            sampleRacketCollider = GetComponent<Collider>();
        }

        private void OnTriggerStay(Collider other) {
            if (isOutOfRange)
                return;
            
            int layer = 1 << other.gameObject.layer;
            if (layer != racketLayer.value) return;

            var distance = Vector3.Distance(sampleRacketCollider.bounds.center, racketCollider.bounds.center);
            PoseDetection(distance);
        }

        private void PoseDetection(float distance) {
            if (!inCenter)
                PoseCorrectionSignal();

            if (distance < .02f)
            {
                inCenter = true;
                PoseCorrectionSignal();
            }
            else
            {
                if (isCorrectPose)
                {
                    racket.ConditionValidation(true);
                }
                else
                {
                    racket.ConditionValidation(false);
                }
            }
        }
        
        private float currentAlignmentScore = 0f;
        private float smoothingFactor = 0.3f; // Adjust for more/less smoothing

        private void PoseCorrectionSignal() {
            // Call racket's method to check alignment, the tolerance is used inside the racket class
            float alignmentScore = racket.CalculateAlignmentScore(transform.parent, playerRacket, meshShape);

            // Smooth the score over time to prevent flickering
            currentAlignmentScore = Mathf.Lerp(currentAlignmentScore, alignmentScore, smoothingFactor);

            // Use smoothed score for feedback
            if (currentAlignmentScore > 0.7f)
            {
                racket.ChangeMaterial(correctMat);
                isCorrectPose = true;
            }
            else
            {
                racket.ChangeMaterial(incorrectMat);
                isCorrectPose = false;
            }
        }

        private void OnTriggerExit(Collider other) {
            //var distance = Vector3.Distance(sampleRacketCollider.bounds.center, racketCollider.bounds.center);
            //PoseDetection(distance);
            ResetState();
        }

        private void ResetState() {
            isOutOfRange = false;
            inCenter = false;
            isCorrectPose = false;
        }
    }
}