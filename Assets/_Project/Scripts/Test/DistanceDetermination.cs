using System;
using UnityEngine;

public class DistanceDetermination : MonoBehaviour
{
    public float value;
    public Transform originalObj;
    public Transform target;

    private void OnDrawGizmos()
    {
        var distanceVector = target.position - originalObj.position;
        value = distanceVector.magnitude;
    }
}
