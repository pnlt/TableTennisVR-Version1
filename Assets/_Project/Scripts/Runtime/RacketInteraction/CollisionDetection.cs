using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

namespace Dorkbots.XR.Runtime.RacketInteraction
{
    public class CollisionDetection : MonoBehaviour
    {
        [Header("Reference components")] 
        [SerializeField] private Transform playerRacket;
        public Collider racketCollider;
        [SerializeField] private MeshFilter meshShape;

        [Header("Materials")] [SerializeField] private Material correctMat;
        [SerializeField] private Material incorrectMat;

        [Header("Threshold")] [SerializeField] private float distanceThreshold;

        private IllustrativeRacket racket;
        private Collider sampleRacketCollider;

        private bool isCorrectPose;
        private bool inCenter;

        private void Awake()
        {
            ReferenceComponents();
        }

        private void ReferenceComponents()
        {
            racket = GetComponentInParent<IllustrativeRacket>();
            sampleRacketCollider = GetComponent<Collider>();
        }

        private void OnTriggerStay(Collider other)
        {
            var distance = Vector3.Distance(sampleRacketCollider.bounds.center, racketCollider.bounds.center);
            
            if (!inCenter)
                PoseCorrectionSignal();

            if (distance < .01f)
            {
                inCenter = true;
                PoseCorrectionSignal();
            }
            else
            {
                if (isCorrectPose)
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
        }

        private void PoseCorrectionSignal()
        {
            if (racket.IsAlignedMesh(transform.parent, playerRacket, meshShape))
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

        private void OnTriggerExit(Collider other)
        {
            ResetState();
        }

        private void ResetState()
        {
            inCenter = false;
            isCorrectPose = false;
        }
    }
}