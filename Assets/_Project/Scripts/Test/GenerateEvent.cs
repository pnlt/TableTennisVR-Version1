using System;
using System.Collections;
using UnityEngine;

public class GenerateEvent : MonoBehaviour
{
    [Header("Materials")]
    public Material correctMaterial;
    public Material incorrectMaterial;
    
    private Material originalMaterial;
    
    [Header("Pose detection")]
    [SerializeField] private float _positionTolerance = 0.05f; // How close positions need to be (in meters)
    [SerializeField] private float _directionTolerance = 0.9f; // Minimum dot product value (0.9 ≈ 25 degrees)
    [SerializeField] private float _overlapCheckRadius = 0.2f; // Radius to check for overlap
    [SerializeField] private LayerMask _playerRacketLayer; 
    
    [Header("Advanced Detection")]
    [SerializeField] private bool _checkForwardDirection = true;  // Check racket face direction
    [SerializeField] private bool _checkUpDirection = true;       // Check racket handle direction
    [SerializeField] private bool _checkRightDirection = true;    // Check racket side direction
    
    [Header("Optional Settings")]
    [SerializeField] private bool _requireContinuousPoseMatch = true; // If true, updates color continuously during overlap
    [SerializeField] private float _goodPoseRetentionTime = 0.5f;
    
    private MeshRenderer _meshRenderer;
    private bool _isOverlapping = false;
    private bool _wasGoodPoseDetected = false;
    private float _goodPoseTimer = 0f;
    private Collider[] _overlapResults = new Collider[10];

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = _meshRenderer.material;
    }

    private void Update()
    {
        int overlappingCount = Physics.OverlapSphereNonAlloc(transform.position, _overlapCheckRadius, _overlapResults, _playerRacketLayer);

        if (!_isOverlapping && overlappingCount > 0)
        {
            _isOverlapping = true;
            CheckPose(_overlapResults[0].transform);
        }
        else if (_isOverlapping && overlappingCount == 0)
        {
            _isOverlapping = false;

            if (_wasGoodPoseDetected && _goodPoseTimer > 0)
            {
                StartCoroutine(RetainGoodPoseCoroutine());
            }
            else
            {
                ResetToDefaultMaterial();
            }
        }
        else if (_isOverlapping && overlappingCount > 0 && _requireContinuousPoseMatch)
        {
            CheckPose(_overlapResults[0].transform);
        }

        if (_wasGoodPoseDetected && _goodPoseTimer > 0)
        {
            _goodPoseTimer -= Time.deltaTime;
            if (_goodPoseTimer <= 0 && !_isOverlapping)
            {
                ResetToDefaultMaterial();
                _wasGoodPoseDetected = false;
            }
        }
    }

    private void CheckPose(Transform playerRacketTransform)
    {
        bool isGoodPose = IsPoseMatching(playerRacketTransform);
        Debug.Log(isGoodPose);
        
        if (isGoodPose)
        {
            _meshRenderer.material = correctMaterial;
            _wasGoodPoseDetected = true;
            _goodPoseTimer = _goodPoseRetentionTime;
        }
        else
        {
            _meshRenderer.material = incorrectMaterial;
        }
    }
    
    private bool IsPoseMatching(Transform playerRacketTransform)
    {
        // Check position match
        float positionDifference = Vector3.Distance(transform.position, playerRacketTransform.position);
        bool positionMatches = positionDifference <= _positionTolerance;
        
        // Initialize direction matching to true
        bool directionsMatch = true;
        
        // Check forward direction (racket face direction)
        if (_checkForwardDirection)
        {
            float forwardDotProduct = Vector3.Dot(transform.forward, playerRacketTransform.forward);
            if (forwardDotProduct < _directionTolerance)
            {
                directionsMatch = false;
            }
        }
        
        // Check up direction (handle direction)
        if (_checkUpDirection && directionsMatch)
        {
            float upDotProduct = Vector3.Dot(transform.up, playerRacketTransform.up);
            if (upDotProduct < _directionTolerance)
            {
                directionsMatch = false;
            }
        }
        
        // Check right direction (side direction)
        if (_checkRightDirection && directionsMatch)
        {
            float rightDotProduct = Vector3.Dot(transform.right, playerRacketTransform.right);
            if (rightDotProduct < _directionTolerance)
            {
                directionsMatch = false;
            }
        }
        
        // For debugging
        if (positionMatches && !directionsMatch)
        {
            Debug.Log("Position matched but directions didn't match");
        }
        
        // Both position and directions need to match
        return positionMatches && directionsMatch;
    }

    private void ResetToDefaultMaterial()
    {
        _meshRenderer.material = originalMaterial;
    }
    
    private IEnumerator RetainGoodPoseCoroutine()
    {
        // Keep the good pose indicator for the retention time
        while (_goodPoseTimer > 0)
        {
            yield return null;
            _goodPoseTimer -= Time.deltaTime;
        }
        
        // After retention time expires, reset
        ResetToDefaultMaterial();
        _wasGoodPoseDetected = false;
    }
}
