using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialSelectionMenu : MonoBehaviour
{
    public OVRInput.Button spawnButton;

    [Range(2, 10)] public int numberOfRadialPart;
    public Transform radialPartCanvas;
    public Transform handTransform;

    private List<GameObject> radialParts = new List<GameObject>();
    private List<IPathTrigger> pathTriggers = new List<IPathTrigger>();
    private List<Color> defaultColors = new List<Color>();
    private IPathTrigger pathTrigger;

    private int currentSelectedRadialPart = -1;

    private void Awake() {
        // Populate the radialParts list with existing radial parts from the canvas
        for (int i = 0; i < numberOfRadialPart; i++)
        {
            Transform child = radialPartCanvas.GetChild(i);
            radialParts.Add(child.gameObject);
            pathTriggers.Add(child.GetComponent<IPathTrigger>());
            defaultColors.Add(child.GetComponent<Image>().color);
        }
    }

    private void Update() {
        if (OVRInput.GetDown(spawnButton))
        {
            radialPartCanvas.gameObject.SetActive(true);
            // Reset all parts to their default state
            for (int i = 0; i < radialParts.Count; i++)
            {
                radialParts[i].GetComponent<Image>().color = defaultColors[i];
                radialParts[i].transform.localScale = Vector3.one;
            }

            currentSelectedRadialPart = -1; // Reset selection
            radialPartCanvas.position = handTransform.position;
            radialPartCanvas.rotation = handTransform.rotation;
        }

        if (OVRInput.Get(spawnButton))
        {
            GetSelectedRadialPart();
        }

        if (OVRInput.GetUp(spawnButton))
        {
            HideAndTriggerSelected();
        }
    }

    private void HideAndTriggerSelected() {
        if (pathTrigger != null && pathTrigger.IsEnabled)
            pathTrigger.OnPathTriggered();

        // Reset the selected part’s visuals
        if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
        {
            radialParts[currentSelectedRadialPart].GetComponent<Image>().color =
                defaultColors[currentSelectedRadialPart];
            radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
        }

        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart() {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.right, centerToHandProjected,
            radialPartCanvas.forward);
        if (angle < 0)
            angle += 360;

        int newSelectedRadialPart = (int)(angle * numberOfRadialPart / 360f);

        // Update visuals only if selection changes
        if (newSelectedRadialPart != currentSelectedRadialPart)
        {
            // Reset the previously selected part
            if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
            {
                radialParts[currentSelectedRadialPart].GetComponent<Image>().color =
                    defaultColors[currentSelectedRadialPart];
                radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
            }

            // Update to the new selection
            currentSelectedRadialPart = newSelectedRadialPart;
            pathTrigger = pathTriggers[currentSelectedRadialPart];

            // Highlight the new part only if it’s enabled
            if (pathTrigger.IsEnabled)
            {
                //radialParts[currentSelectedRadialPart].GetComponent<Image>().color = Color.red;
                radialParts[currentSelectedRadialPart].transform.localScale = 1.1f * Vector3.one;
            }
        }
    }

    //method to enable a specific radial part by index
    public void EnableRadialPart(int index) {
        if (index >= 0 && index < pathTriggers.Count)
        {
            pathTriggers[index].IsEnabled = true;
        }
    }
}