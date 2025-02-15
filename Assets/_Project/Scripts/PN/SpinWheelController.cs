using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SpinWheelController : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Wheel Configuration")]
    [Tooltip("Constraint rotation to Z-axis only")]
    [SerializeField] private bool lockRotationToZAxis = true;
    
    [Tooltip("Drag coefficient to simulate air resistance")]
    [SerializeField] private float rotationalDrag = 0.5f;
    
    [Tooltip("Minimum force required to start rotation")]
    [SerializeField] private float minimumForceThreshold = 1f;
    
    [Tooltip("Maximum angular velocity allowed")]
    [SerializeField] private float maxAngularVelocity = 25f;
    
    [Header("Physics Tuning")]
    [Tooltip("Multiplier for impact force")]
    [SerializeField] private float forceMultiplier = 1.5f;
    
    [Tooltip("How quickly the wheel slows down")]
    [SerializeField] private float dampingForce = 0.98f;
    
    [Header("Events")]
    public UnityEvent<float> OnSpinSpeedChanged;
    public UnityEvent OnSpinStopped;
    
    // Threshold for considering the wheel stopped
    private const float STOP_THRESHOLD = 0.01f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitializeRigidbody();
    }

    private void InitializeRigidbody()
    {
        // Optimize rigidbody settings for rotation
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | 
                        RigidbodyConstraints.FreezeRotationX | 
                        RigidbodyConstraints.FreezeRotationY;
        
        rb.angularDamping = rotationalDrag;
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        HandleRotationDamping();
        CheckForStop();
    }

    private void HandleRotationDamping()
    {
        if (Mathf.Abs(rb.angularVelocity.z) > 0)
        {
            // Apply smooth damping
            rb.angularVelocity *= dampingForce;
            OnSpinSpeedChanged?.Invoke(Mathf.Abs(rb.angularVelocity.z));
        }
    }

    private void CheckForStop()
    {
        if (Mathf.Abs(rb.angularVelocity.z) < STOP_THRESHOLD && rb.angularVelocity.z != 0)
        {
            rb.angularVelocity = Vector3.zero;
            OnSpinStopped?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleImpact(collision);
    }

    private void HandleImpact(Collision collision)
    {
        // Get the first contact point
        ContactPoint contact = collision.GetContact(0);
        
        // Calculate the impact force
        Vector3 impactForce = collision.impulse / Time.fixedDeltaTime;
        float impactMagnitude = impactForce.magnitude;

        // Ignore weak impacts
        if (impactMagnitude < minimumForceThreshold)
            return;

        // Calculate the torque direction based on impact point
        Vector3 contactPoint = contact.point - transform.position;
        Vector3 crossProduct = Vector3.Cross(contactPoint.normalized, contact.normal);
        
        // Apply torque in the correct direction with proper force
        float torqueMagnitude = impactMagnitude * forceMultiplier;
        Vector3 torque = new Vector3(0, 0, crossProduct.z * torqueMagnitude);
        
        // Apply the torque
        rb.AddTorque(torque, ForceMode.Impulse);
    }

    // Public method to get current rotation speed
    public float GetCurrentSpeed()
    {
        return Mathf.Abs(rb.angularVelocity.z);
    }

    // Optional: Method to apply programmatic spin
    public void ApplySpinForce(float force, bool clockwise = true)
    {
        float direction = clockwise ? -1f : 1f;
        rb.AddTorque(new Vector3(0, 0, force * direction), ForceMode.Impulse);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure values stay within reasonable ranges
        rotationalDrag = Mathf.Max(0, rotationalDrag);
        minimumForceThreshold = Mathf.Max(0, minimumForceThreshold);
        maxAngularVelocity = Mathf.Max(1, maxAngularVelocity);
        dampingForce = Mathf.Clamp(dampingForce, 0.9f, 0.999f);
    }

    private void OnDrawGizmos()
    {
        // Visualize rotation axis
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 2f);
    }
#endif
}
