using System;
using Dorkbots.XR.Runtime.SoundAndSFX;

public class FirstRacketIllustrationPoint : IllustrativeRacket 
{
    private void Update()
    {
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
