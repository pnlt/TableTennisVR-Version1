using System.Collections.Generic;
using Dorkbots.XR.Runtime;
using UnityEngine;

public class RadialSelectionMenu : MonoBehaviour
{
    public OVRInput.Button spawnButton;

    [Range(2, 10)] public int numberOfRadialPart;
    public Transform radialPartCanvas;
    public Transform handTransform;

    private List<GameObject> radialParts = new();
    private List<IPathTrigger> pathTriggers = new();
    private IPathTrigger pathTrigger;
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

    private void OnEnable() {
        DisableChallengePathEvent.Subscribe(DisableChallengePart);
    }

    private void Start() {
        /*if (LevelManager.Instance != null)
        {
            int level = LevelManager.Instance.levelNumber;
            int challengeIndex = GetChallengePartIndex();
            if (challengeIndex != -1)
            {
                bool practiceCompleted = PlayerPrefs.GetInt("PracticeCompleted_Level" + level, 0) == 1;
                bool challengeCompleted = PlayerPrefs.GetInt("ChallengeCompleted_Level" + level, 0) == 1;
                pathTriggers[challengeIndex].IsEnabled = practiceCompleted && !challengeCompleted;
            }
        }*/
    }

    private void DisableChallengePart() {
        int challengeIndex = GetChallengePartIndex();
        if (challengeIndex != -1)
        {
            pathTriggers[challengeIndex].IsEnabled = false;
        }
    }

    private int GetChallengePartIndex() {
        for (int i = 0; i < pathTriggers.Count; i++)
        {
            if (pathTriggers[i] is ChallengePathTrigger)
            {
                return i;
            }
        }

        return -1; // Return -1 if no ChallengePathTrigger is found
    }

    private void Update() {
        if (OVRInput.GetDown(spawnButton))
        {
            radialPartCanvas.gameObject.SetActive(true);
            // Reset all parts to their default state
            for (int i = 0; i < radialParts.Count; i++)
            {
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
            radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
        }

        radialPartCanvas.gameObject.SetActive(false);
    }

    private void GetSelectedRadialPart() {
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;
        
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.right, centerToHandProjected,
            radialPartCanvas.forward);
        if (angle < 0)
            angle += 360;

        var testVal = centerToHand.magnitude;
        if (testVal > 0.05f)
            UIManager.Instance.SetValueDebug("center");

        int newSelectedRadialPart = (int)(angle * numberOfRadialPart / 360f);

        // Update visuals only if selection changes
        if (newSelectedRadialPart != currentSelectedRadialPart)
        {
            // Reset the previously selected part
            if (currentSelectedRadialPart >= 0 && currentSelectedRadialPart < radialParts.Count)
            {
                radialParts[currentSelectedRadialPart].transform.localScale = Vector3.one;
            }

            // Update to the new selection
            currentSelectedRadialPart = newSelectedRadialPart;
            pathTrigger = pathTriggers[newSelectedRadialPart];

            // Highlight the new part only if it’s enabled
            if (pathTrigger.IsEnabled)
            {
                radialParts[currentSelectedRadialPart].transform.localScale = 1.1f * Vector3.one;
            }
        }
    }

    private void OnDisable() {
        DisableChallengePathEvent.Unsubscribe(DisableChallengePart);
    }
}