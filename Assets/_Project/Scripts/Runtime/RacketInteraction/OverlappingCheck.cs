using System;
using UnityEngine;

public class OverlappingCheck : MonoBehaviour
{
    [Header ("Reference components")]
    [SerializeField] private Transform playerRacket;
    [SerializeField] private MeshFilter meshShape;
    
    [Header ("Accurate level")]
    [SerializeField] private float tolerance;

    private void Update()
    {
        if (IsAlignedMesh(transform, playerRacket, meshShape))
        {
            
        }
    }

    private bool IsAlignedMesh(Transform original, Transform racket, MeshFilter mesh)
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
