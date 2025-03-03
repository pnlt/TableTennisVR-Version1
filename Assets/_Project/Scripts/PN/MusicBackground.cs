using System;
using AudioSystem;
using UnityEngine;

public class MusicBackground : MonoBehaviour
{
    [SerializeField] SoundData soundData;

    private void Start()
    {
    SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
            
                    soundBuilder
                        .WithRandomPitch()
                        .WithPosition(transform.position)
                        .Play(soundData);
        
    }

}
