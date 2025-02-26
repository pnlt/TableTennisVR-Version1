using System;
using UnityEngine;

public class CoverLine : MonoBehaviour
{
    public LayerMask racketLayer;

    private void OnTriggerEnter(Collider collider)
    {
        var targetLayer = 1 << collider.gameObject.layer;
        if (targetLayer == racketLayer.value)
        {
            // Only add to the list if it's not already in there
            if (!GuideLine.CoverLines.Contains(this))
            {
                GuideLine.AddElement(this);
            }
        
            // Disable the line when hit
            EnablingObject(false);
        
            // Notify the game system about this hit
            // This will be important for triggering reset logic
            GuideLine.NotifyLineHit(this);
        }
    }


    public void EnablingObject(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
