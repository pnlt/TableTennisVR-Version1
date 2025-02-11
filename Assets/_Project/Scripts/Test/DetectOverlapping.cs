using System;
using UnityEngine;

public class DetectOverlapping : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Transform transparentObject;
    public MeshFilter transparentMeshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        transparentMeshFilter = transparentObject.GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var vertex in meshFilter.mesh.vertices)
        {
            Vector3 p = transform.TransformPoint(vertex);
        }
        
    }
}
