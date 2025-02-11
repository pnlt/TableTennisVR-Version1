using UnityEngine;

[CreateAssetMenu(fileName = "RacketData", menuName = "RacketCoordinateStore")]
public class RacketCoordinateSO : ScriptableObject
{
    public Vector3 racketCoordinatePos;
    public Vector3 racketCoordinateRotation;
}
