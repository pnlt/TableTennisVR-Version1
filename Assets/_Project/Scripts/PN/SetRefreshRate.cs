using UnityEngine;

public class SetRefreshRate : MonoBehaviour
{
    void Start() {
        Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(120f);
    }
}