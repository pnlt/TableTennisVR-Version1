using System.Collections.Generic;
using Oculus.Interaction.OVR.Input;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialSelectionMenu : MonoBehaviour
{
    public OVRInput.Button spawnButton;

    [Range(2, 10)] public int numberOfRadialPart;
    public Transform radialPartCanvas;
    public Transform handTransform;

    private IPathTrigger pathTrigger;

    private List<GameObject> radialParts = new List<GameObject>();
    private List<IPathTrigger> pathTriggers = new List<IPathTrigger>();
    private int currentSelectedRadialPart = -1;

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
        if (OVRInput.GetDown(spawnButton))
        {
            radialPartCanvas.gameObject.SetActive(true);
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
        if (pathTrigger != null)
            pathTrigger.OnPathTriggered();

        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart() {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.right, centerToHandProjected,
            radialPartCanvas.forward);
        if (angle < 0)
            angle += 360;

        currentSelectedRadialPart = (int)(angle * numberOfRadialPart / 360f);
        pathTrigger = pathTriggers[currentSelectedRadialPart];

        for (int i = 0; i < radialParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                radialParts[i].transform.localScale = 1.1f * Vector3.one;
            }
            else
            {
                radialParts[i].transform.localScale = Vector3.one;
            }
        }
    }
}