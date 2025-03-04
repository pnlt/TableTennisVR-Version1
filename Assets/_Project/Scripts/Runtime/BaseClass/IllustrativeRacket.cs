using UnityEngine;

namespace Dorkbots.XR.Runtime.SoundAndSFX
{
    public class IllustrativeRacket : MonoBehaviour
    {
        [Header("Reference components")] 
        [SerializeField] protected Transform playerRacket;
        [SerializeField] protected Material racketMaterial;
        [SerializeField] protected MeshFilter meshShape;

        [Header("Accurate level")] [SerializeField]
        private float tolerance;

        [Header("Assets alteration")] 
        [SerializeField] protected Material correctMat;
        [SerializeField] protected Material incorrectMat;

        protected bool IsAlignedMesh(Transform original, Transform racket, MeshFilter mesh)
        {
            Mesh meshes = mesh.sharedMesh;
            Vector3[] vertices = meshes.vertices;

            foreach (Vector3 vertex in vertices)
            {
                Vector3 worldVertexA = original.TransformPoint(vertex);
                Vector3 worldVertexB = racket.TransformPoint(vertex);

                if (Vector3.Distance(worldVertexA, worldVertexB) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}