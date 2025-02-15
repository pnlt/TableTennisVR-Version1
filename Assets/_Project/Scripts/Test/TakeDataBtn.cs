using UnityEngine;

public class TakeDataBtn : MonoBehaviour
{
    public RacketCoordinateSO racketCoordinate;
    public Transform RacketTransform;
    
    public void OnClicked()
    {
        racketCoordinate.racketCoordinateRotation = RacketTransform.eulerAngles;
    }
}
