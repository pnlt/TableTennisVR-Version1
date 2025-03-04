using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

namespace Dorkbots.XR.Runtime.RacketInteraction
{
        
    public class CollisionDetection : MonoBehaviour
    {
        [Header("Reference components")] 
        [SerializeField] protected Transform playerRacket;
        [SerializeField] protected MeshFilter meshShape;
        

        [Header("Assets alteration")] 
        [SerializeField] protected Material correctMat;
        [SerializeField] protected Material incorrectMat;
        public IllustrativeRacket racket;
        public Collider racketCollider;

        private void Awake()
        {
            racket = GetComponentInParent<IllustrativeRacket>();
            racketCollider = GetComponentInParent<Collider>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (racket.IsAlignedMesh(transform.parent, playerRacket, meshShape))
            {
                racket.ChangeMaterial(correctMat);
            }
            else
            {
                racket.ChangeMaterial(incorrectMat);
            }
        }
    }
}