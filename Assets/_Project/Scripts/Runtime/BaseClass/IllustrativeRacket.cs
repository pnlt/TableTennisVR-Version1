using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dorkbots.XR.Runtime.SoundAndSFX
{
    public abstract class IllustrativeRacket : MonoBehaviour
    {
        [Header("Accurate level")] [SerializeField]
        private float tolerance;

        [SerializeField] protected MeshRenderer racketRender;
        [SerializeField] protected ScoreInSampleRacket scoreSampleRacket;

        protected Material originalMat;
        protected static bool isCorrectPose;

        protected void Awake() {
            ReferenceComponents();
            originalMat = racketRender.material;
        }

        public virtual void ConditionValidation(bool condition) {
            isCorrectPose = condition;
        }

        protected virtual void ReferenceComponents() {
            scoreSampleRacket = GetComponentInParent<ScoreInSampleRacket>();
        }

        // Add this new method
        public float CalculateAlignmentScore(Transform original, Transform racket, MeshFilter mesh) {
            Mesh meshes = mesh.sharedMesh;
            Vector3[] vertices = meshes.vertices;

            if (vertices.Length == 0) return 0;

            int matchingVertices = 0;
            int sampleSize = Mathf.Min(vertices.Length, 20); // Limit to checking only 20 vertices for performance
            int step = vertices.Length / sampleSize;

            for (int i = 0; i < vertices.Length; i += step)
            {
                if (i >= vertices.Length) break;

                Vector3 vertex = vertices[i];
                Vector3 worldVertexA = original.TransformPoint(vertex);
                Vector3 worldVertexB = racket.TransformPoint(vertex);

                // Here we use the tolerance defined in this class
                if (Vector3.Distance(worldVertexA, worldVertexB) <= tolerance * 1.5f)
                {
                    matchingVertices++;
                }
            }

            // Calculate percentage of matching vertices
            return (float)matchingVertices / sampleSize;
        }

        public abstract void SetMatToOrigin();

        public void ChangeMaterial(Material material) {
            racketRender.material = material;
        }
    }
}