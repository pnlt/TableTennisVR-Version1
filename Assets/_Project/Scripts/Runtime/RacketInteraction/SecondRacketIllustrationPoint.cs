using System;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

public class SecondRacketIllustrationPoint : IllustrativeRacket
{
    private void OnTriggerStay(Collider other)
    {
        if (IsAlignedMesh(gameObject.transform, playerRacket, meshShape))
        {
            ChangeMaterial(correctMat);
        }
    }
}