using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;

// Perhaps we have to create a base class for two types of audio Music and SFX

public class MusicAudio : BaseAudio
{
   protected override void Awake()
   {
      audioSource = GetComponent<AudioSource>();
      base.Awake();
   }
}
