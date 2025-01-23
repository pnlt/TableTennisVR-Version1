using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class FormingSpline : MonoBehaviour
{
    private SplineContainer _spline;
    public Vector3[] Points;
    private int knotNum;

    [ContextMenu("run")]
    public void Execute()
    {
        _spline = GetComponent<SplineContainer>();
        knotNum = _spline.Spline.ToArray().Length;

        for (int i = 0; i < knotNum; i++)
        {
            var knot = _spline.Spline.ToArray()[i];
            knot.Position = transform.InverseTransformPoint(Points[i]);
            _spline.Spline.SetKnot(i, knot);
        }
    }
}
