using System.Collections.Generic;
using Oculus.Interaction.OVR.Input;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialSelectionWithIcons : MonoBehaviour
{
    [System.Serializable]
    public class RadialPartData
    {
        public Sprite icon;
        public Color segmentColor = Color.white;
    }

    public OVRInput.Button spawnButton;

    [Range(2, 10)] public int numberOfRadialPart = 4;

    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;

    [Range(0f, 45f)] public float angleBetweenPart = 10f;

    public Transform handTransform;

    // List to define icons and colors for each segment
    public List<RadialPartData> segmentIcons;

    public UnityEvent<int> OnPartSelected;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentSelectedRadialPart = -1;

    private void Update() {
        if (OVRInput.GetDown(spawnButton))
        {
            SpawnRadialPart();
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

    public void HideAndTriggerSelected() {
        OnPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart() {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);
        if (angle < 0)
            angle += 360;

        currentSelectedRadialPart = Mathf.FloorToInt(angle * numberOfRadialPart / 360f);

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                spawnedParts[i].transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialPart() {
        // Ensure we have enough segment data
        while (segmentIcons.Count < numberOfRadialPart)
        {
            segmentIcons.Add(new RadialPartData());
        }

        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;

        // Destroy existing parts
        foreach (var item in spawnedParts)
        {
            Destroy(item);
        }

        spawnedParts.Clear();

        // Spawn new parts
        for (int i = 0; i < numberOfRadialPart; i++)
        {
            float angle = -i * (360f / numberOfRadialPart) - (angleBetweenPart / 2f);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);

            // Position and rotate the radial part
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = new Vector3(0, 0, angle);

            // Configure the image fill and color
            Image radialImage = spawnedRadialPart.GetComponent<Image>();
            if (radialImage != null)
            {
                radialImage.fillAmount = (1f / numberOfRadialPart) - (angleBetweenPart / 360f);
                radialImage.color = segmentIcons[i].segmentColor;
            }

            // Set icon for the segment
            Transform iconTransform = spawnedRadialPart.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && segmentIcons[i].icon != null)
                {
                    iconImage.sprite = segmentIcons[i].icon;
                    iconImage.preserveAspect = true;
                }

                iconTransform.localPosition = Vector3.zero;
            }

            spawnedParts.Add(spawnedRadialPart);
        }
    }
}