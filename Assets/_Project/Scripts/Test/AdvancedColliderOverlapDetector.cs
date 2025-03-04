using UnityEngine;

public class AdvancedColliderOverlapDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Transform _trackedObject;
    [SerializeField] private float _exitThreshold = 0.5f; // Percentage of collider that must be outside
    [SerializeField] private int _samplePoints = 10; // Number of sample points to use for penetration testing
    [SerializeField] private bool _debugMode = true;
    
    private Collider _thisCollider;
    public Collider _otherCollider;
    private bool _isPartiallyExited = false;
    private Vector3[] _samplePointsArray;
    
    private void Start()
    {
        _thisCollider = GetComponent<Collider>();
        if (_thisCollider == null)
        {
            Debug.LogError("No collider found on this object!");
            enabled = false;
            return;
        }
        
        // Initialize sample points array
        InitializeSamplePoints();
    }
    
    private void InitializeSamplePoints()
    {
        if (_thisCollider is MeshCollider meshCollider && !meshCollider.convex)
        {
            Debug.LogWarning("Non-convex mesh colliders are not supported for sampling!");
            return;
        }
        
        _samplePointsArray = new Vector3[_samplePoints * _samplePoints * _samplePoints];
        
        // Create a grid of sample points within the collider bounds
        Bounds bounds = _thisCollider.bounds;
        Vector3 size = bounds.size;
        Vector3 min = bounds.min;
        
        int index = 0;
        for (int x = 0; x < _samplePoints; x++)
        {
            for (int y = 0; y < _samplePoints; y++)
            {
                for (int z = 0; z < _samplePoints; z++)
                {
                    // Calculate position of this sample point
                    Vector3 localPos = new Vector3(
                        min.x + size.x * ((float)x / (_samplePoints - 1)),
                        min.y + size.y * ((float)y / (_samplePoints - 1)),
                        min.z + size.z * ((float)z / (_samplePoints - 1))
                    );
                    
                    _samplePointsArray[index] = localPos;
                    index++;
                }
            }
        }
    }
    
    private void Update()
    {
        if (_trackedObject == null || _samplePointsArray == null) return;
        
        // Find the collider we're tracking
        if (_otherCollider == null)
        {
            _otherCollider = _trackedObject.GetComponent<Collider>();
            if (_otherCollider == null)
            {
                Debug.LogWarning("No collider found on tracked object!");
                return;
            }
        }
        
        // Calculate the percentage of points inside the other collider
        float overlapPercentage = CalculateOverlapPercentage();
        
        // Check if we've crossed the threshold
        if (!_isPartiallyExited && overlapPercentage < (1 - _exitThreshold))
        {
            _isPartiallyExited = true;
            
            if (_debugMode)
            {
                Debug.Log($"Partial exit detected! Overlap: {overlapPercentage:P2}");
            }
        }
        else if (_isPartiallyExited && overlapPercentage >= (1 - _exitThreshold))
        {
            _isPartiallyExited = false;
            
            if (_debugMode)
            {
                Debug.Log($"Object returned to full overlap. Overlap: {overlapPercentage:P2}");
            }
        }
        
        if (_debugMode)
        {
            DrawDebugPoints();
        }
    }
    
    private float CalculateOverlapPercentage()
    {
        if (_samplePointsArray == null || _samplePointsArray.Length == 0)
        {
            return 0f;
        }
        
        int pointsInside = 0;
        
        foreach (Vector3 point in _samplePointsArray)
        {
            // Check if this point is inside the other collider
            if (IsPointInCollider(point, _otherCollider))
            {
                pointsInside++;
            }
        }
        
        return (float)pointsInside / _samplePointsArray.Length;
    }
    
    private bool IsPointInCollider(Vector3 point, Collider collider)
    {
        return collider.ClosestPoint(point) == point;
    }
    
    private void DrawDebugPoints()
    {
        if (_samplePointsArray == null) return;
        
        foreach (Vector3 point in _samplePointsArray)
        {
            bool isInside = _otherCollider != null && IsPointInCollider(point, _otherCollider);
            Color color = isInside ? Color.green : Color.red;
            Debug.DrawRay(point, Vector3.up * 0.01f, color);
        }
    }
    
    // Optional: To use the Unity Physics.ComputePenetration method for more precise results
    private bool CheckPenetration(Collider a, Collider b)
    {
        Vector3 direction;
        float distance;
        
        bool isPenetrating = Physics.ComputePenetration(
            a, a.transform.position, a.transform.rotation,
            b, b.transform.position, b.transform.rotation,
            out direction, out distance
        );
        
        return isPenetrating;
    }
}
