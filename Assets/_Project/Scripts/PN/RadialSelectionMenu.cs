using System.Collections.Generic;
using UnityEngine;

public class RadialSelectionMenu : MonoBehaviour
{
    public OVRInput.Button spawnButton;

    [Range(2, 10)] public int numberOfRadialPart;
    public Transform radialPartCanvas;
    public Transform handTransform;

    [Header("Menu Positioning")] public float menuDistanceFromHand = 0.2f; // Offset distance from hand
    public Vector3 menuPositionOffset = new Vector3(0, 0, 0.2f); // Additional offset if needed

    [Header("Selection Settings")] public float selectionDeadZone = 0.05f; // Minimum distance to select
    public float selectionHighlightScale = 1.1f; // Scale factor for highlighting

    private List<GameObject> radialParts = new();
    private List<IPathTrigger> pathTriggers = new();
    private IPathTrigger pathTrigger;
    private int currentSelectedRadialPart = -1;
    private Vector3 initialHandPosition; // Track initial hand position when menu opened
    private bool menuActive = false;

    private void Awake() {
        // Populate the radialParts list with existing radial parts from the canvas
        for (int i = 0; i < numberOfRadialPart; i++)
        {
            Transform child = radialPartCanvas.GetChild(i);
            radialParts.Add(child.gameObject);
            pathTriggers.Add(child.GetComponent<IPathTrigger>());
        }
    }

    private void Update() {
        if (OVRInput.GetDown(spawnButton) && !menuActive)
        {
            ShowMenu();
        }

        if (OVRInput.Get(spawnButton) && menuActive)
        {
            GetSelectedRadialPart();
        }

        if (OVRInput.GetUp(spawnButton) && menuActive)
        {
            HideAndTriggerSelected();
        }
    }

    private void ShowMenu() {
        // Store initial hand position for reference
        initialHandPosition = handTransform.position;

        // Position the menu with offset from hand
        Vector3 menuPosition = handTransform.position + handTransform.TransformDirection(menuPositionOffset);

        // Ensure the menu is at a comfortable distance from the hand
        Vector3 directionFromHand = (menuPosition - handTransform.position).normalized;
        menuPosition = handTransform.position + directionFromHand * menuDistanceFromHand;

        // Set position and rotation
        radialPartCanvas.position = menuPosition;
        radialPartCanvas.rotation = handTransform.rotation;

        // Reset all parts to their default state
        for (int i = 0; i < radialParts.Count; i++)
        {
            radialParts[i].transform.localScale = Vector3.one;
        }

        currentSelectedRadialPart = -1; // Reset selection
        radialPartCanvas.gameObject.SetActive(true);
        menuActive = true;
    }

    private void HideAndTriggerSelected() {
        if (pathTrigger != null && pathTrigger.IsEnabled && currentSelectedRadialPart != -1)
            pathTrigger.OnPathTriggered();

        if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
        {
            radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
        }

        radialPartCanvas.gameObject.SetActive(false);
        menuActive = false;
    }


    private void GetSelectedRadialPart() {
        // Calculate current hand movement from initial position
        Vector3 handMovement = handTransform.position - initialHandPosition;

        // Project the hand movement onto the menu plane
        Vector3 handMovementProjected = Vector3.ProjectOnPlane(handMovement, radialPartCanvas.forward);

        // Check if hand has moved enough to make a selection
        if (handMovementProjected.magnitude < selectionDeadZone)
        {
            // If hand hasn't moved enough, clear selection
            if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
            {
                radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
            }

            currentSelectedRadialPart = -1;
            pathTrigger = null;
            return;
        }

        // Calculate angle for selection
        float angle = Vector3.SignedAngle(radialPartCanvas.right, handMovementProjected, radialPartCanvas.forward);
        if (angle < 0)
            angle += 360;

        // Calculate which radial part is selected
        int newSelectedRadialPart = (int)(angle * numberOfRadialPart / 360f);

        // Update selection if changed
        if (newSelectedRadialPart != currentSelectedRadialPart)
        {
            // Reset previous selection
            if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
            {
                radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
            }

            // Update current selection
            currentSelectedRadialPart = newSelectedRadialPart;
            pathTrigger = pathTriggers[currentSelectedRadialPart];

            // Highlight if enabled
            if (pathTrigger.IsEnabled)
            {
                radialParts[currentSelectedRadialPart].transform.localScale = selectionHighlightScale * Vector3.one;
            }
        }
    }
}