using UnityEngine;

public class SetRefreshRateLevelSelection : MonoBehaviour
{
    void Start() {
        Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90f);
    }
}