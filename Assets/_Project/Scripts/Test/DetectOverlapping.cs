using System;
using UnityEngine;

public class DetectOverlapping : MonoBehaviour
{
    public Transform target;
    public MeshFilter meshFilter;

    private void Update()
    {
        if (Overlapping(this.transform, target, meshFilter))
        {
            Debug.Log(this.transform.name + " is overlapping.");
        }
        else
        {
            Debug.Log(this.transform.name + " is not overlapping.");
        }
    }

    private bool Overlapping(Transform original, Transform target, MeshFilter meshFilter)
    {
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        
        foreach (Vector3 vertex in vertices)
        {
            var worldVertexA = original.TransformPoint(vertex);
            var worldVertexB = target.TransformPoint(vertex);

            if (Vector3.Distance(worldVertexA, worldVertexB) > 0.15f)
            {
                return false;
            }
        }
        
        return true;
    }
}
