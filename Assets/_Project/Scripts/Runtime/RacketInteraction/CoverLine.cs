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
            GuideLine.AddElement(this);
            // When player add force to racket following line, there are two cases: 
            
            // Case 1: Player completed perfect Line => The cover line will be reset gradually after disabling 
            EnablingObject(false);
            
            //Case 2: Player is moving racket following the line then moving accidentally in a wrong direction
            // => Show a notification about player's fouls
            // => Reset all the disabled cover line player have already gone through
        }
    }

    public void EnablingObject(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
