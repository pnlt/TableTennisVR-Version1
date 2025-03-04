using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dorkbots.XR.Runtime.SoundAndSFX
{
    public class IllustrativeRacket : MonoBehaviour
    {
        [Header("Accurate level")] 
        [SerializeField] private float tolerance;
        [SerializeField] protected MeshRenderer racketRender;
        
        protected Material originalMat;

        protected void Start()
        {
        }
        

        public bool IsAlignedMesh(Transform original, Transform racket, MeshFilter mesh)
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

        public void ChangeMaterial(Material material)
        {
            racketRender.material = material;
        }
    }
}