using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    private void OnTriggerStay(Collider other)
    {
        // Determine if Racket collided with sample one
        if (IsAlignedMesh(gameObject.transform, playerRacket, meshShape))
        {
            // Change mat to green color
            racketMaterial = correctMat;
        }
        else
        {
            // Change mat to red color
            racketMaterial = incorrectMat;
        }
    }
}
