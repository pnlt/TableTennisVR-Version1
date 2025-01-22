using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PaddleControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject paddle;
    [SerializeField] private GameObject avatar;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spinWheel;

    [Header("Spin Wheel Settings")]
    [SerializeField] private float spinForceMultiplier = 5f;
    [SerializeField] private float maxSpinVelocity = 1000f;
    [SerializeField] private float spinDecayRate = 0.98f;
    [SerializeField] private float wheelDetectionRadius = 0.5f;

    [Header("Paddle Settings")]
    [SerializeField] private float handOffset = 0.1f;
    [SerializeField] private float paddleRotationAngle = 45f;
    [SerializeField] private float triggerThreshold = 0.2f;
    [SerializeField] private Vector3 handColliderSize = new Vector3(0.2f, 0.2f, 0.2f);

    // Cached references
    private GameObject rightHand;
    private GameObject leftHand;
    private Rigidbody paddleRigidbody;
    private Rigidbody wheelRigidbody;
    private GameObject activeHand;
    
    // State tracking
    private bool isHoldingPaddleRight;
    private bool isHoldingPaddleLeft;
    private bool isInitialized;
    private Vector3 lastPaddlePosition;
    private float currentSpinVelocity;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (!paddle) paddle = gameObject;
        paddleRigidbody = paddle.GetComponent<Rigidbody>();
        
        if (spinWheel != null)
        {
            wheelRigidbody = spinWheel.GetComponent<Rigidbody>();
            if (wheelRigidbody)
            {
                wheelRigidbody.maxAngularVelocity = maxSpinVelocity;
            }
        }

        isInitialized = false;
        isHoldingPaddleRight = false;
        isHoldingPaddleLeft = false;
        lastPaddlePosition = paddle.transform.position;
    }

    private void FixedUpdate()
    {
        if (!isInitialized)
        {
            InitializeHandColliders();
        }

        UpdatePaddleState();
        UpdateSpinWheelPhysics();
    }

    private void InitializeHandColliders()
    {
        if (avatar == null || avatar.transform.childCount <= 1) return;

        leftHand = avatar.transform.GetChild(0).gameObject;
        rightHand = avatar.transform.GetChild(1).gameObject;

        SetupHandCollider(leftHand, "LeftHand");
        SetupHandCollider(rightHand, "RightHand");

        isInitialized = true;
    }

    private void SetupHandCollider(GameObject hand, string handName)
    {
        if (!hand.TryGetComponent<BoxCollider>(out var collider))
        {
            collider = hand.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = handColliderSize;
        }
        hand.name = handName;
    }

    private void UpdatePaddleState()
    {
        if (!avatar || avatar.transform.childCount <= 1) return;

        avatar.transform.position = player.transform.position;

        // Handle right hand input
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > triggerThreshold && activeHand?.name == "RightHand")
        {
            HandlePaddleHolding(rightHand, OVRInput.Controller.RTouch, true);
        }
        // Handle left hand input
        else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > triggerThreshold && activeHand?.name == "LeftHand")
        {
            HandlePaddleHolding(leftHand, OVRInput.Controller.LTouch, false);
        }
        else
        {
            ReleasePaddle();
        }

        // Update last position for velocity calculation
        lastPaddlePosition = paddle.transform.position;
    }

    private void HandlePaddleHolding(GameObject hand, OVRInput.Controller controller, bool isRightHand)
    {
        paddleRigidbody.isKinematic = true;
        paddleRigidbody.useGravity = false;

        // Update position and rotation
        Vector3 targetPosition = hand.transform.position + paddle.transform.forward * -handOffset;
        paddleRigidbody.MovePosition(targetPosition);
        paddleRigidbody.MoveRotation(hand.transform.rotation);
        
        // Apply paddle rotation offset
        paddle.transform.Rotate(new Vector3(-paddleRotationAngle, 0, 0));
        
        // Update velocity from controller
        paddleRigidbody.linearVelocity = OVRInput.GetLocalControllerVelocity(controller);

        // Update holding state
        isHoldingPaddleRight = isRightHand;
        isHoldingPaddleLeft = !isRightHand;

        // Check for wheel interaction
        CheckWheelInteraction();
    }

    private void CheckWheelInteraction()
    {
        if (wheelRigidbody != null && 
            Vector3.Distance(spinWheel.transform.position, paddle.transform.position) < wheelDetectionRadius)
        {
            ProcessWheelInteraction();
        }
    }

    private void ProcessWheelInteraction()
    {
        // Calculate paddle velocity
        Vector3 paddleVelocity = (paddle.transform.position - lastPaddlePosition) / Time.fixedDeltaTime;
        
        // Calculate impact point and direction
        Vector3 hitPoint = paddle.transform.position;
        Vector3 wheelCenter = spinWheel.transform.position;
        Vector3 radiusVector = hitPoint - wheelCenter;
        
        // Calculate tangential force
        Vector3 tangentialDirection = Vector3.Cross(spinWheel.transform.up, radiusVector).normalized;
        float tangentialSpeed = Vector3.Dot(paddleVelocity, tangentialDirection);
        
        // Apply spin force
        float spinForce = tangentialSpeed * spinForceMultiplier;
        wheelRigidbody.AddTorque(spinWheel.transform.up * spinForce, ForceMode.Impulse);
        
        // Optional: Add haptic feedback
        if (isHoldingPaddleRight)
        {
            OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
        }
        else if (isHoldingPaddleLeft)
        {
            OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
        }
    }

    private void UpdateSpinWheelPhysics()
    {
        if (wheelRigidbody != null)
        {
            // Apply spin decay
            wheelRigidbody.angularVelocity *= spinDecayRate;
            
            // Clamp maximum spin velocity
            if (wheelRigidbody.angularVelocity.magnitude > maxSpinVelocity)
            {
                wheelRigidbody.angularVelocity = wheelRigidbody.angularVelocity.normalized * maxSpinVelocity;
            }
        }
    }

    private void ReleasePaddle()
    {
        paddleRigidbody.isKinematic = false;
        paddleRigidbody.useGravity = true;
        isHoldingPaddleRight = false;
        isHoldingPaddleLeft = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activeHand == null)
        {
            if (other.name == "RightHand") activeHand = rightHand;
            else if (other.name == "LeftHand") activeHand = leftHand;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.name == "RightHand" && activeHand?.name == "RightHand") ||
            (other.name == "LeftHand" && activeHand?.name == "LeftHand"))
        {
            activeHand = null;
        }
    }

    // Public properties for external access
    public bool IsHoldingPaddleRight => isHoldingPaddleRight;
    public bool IsHoldingPaddleLeft => isHoldingPaddleLeft;
    public bool IsPaddleHeld => isHoldingPaddleRight || isHoldingPaddleLeft;
    public float CurrentSpinVelocity => wheelRigidbody ? wheelRigidbody.angularVelocity.magnitude : 0f;
}