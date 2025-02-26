using _Project.Scripts.Runtime.Enum;
using Dorkbots.XR.Runtime.SoundAndSFX;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Audio : BaseAudio
{
    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        base.Awake();
    }

    public void PlayClip(SoundTypes audioType, Vector3 position)
    {
        if (audioClipDict.TryGetValue(audioType, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
