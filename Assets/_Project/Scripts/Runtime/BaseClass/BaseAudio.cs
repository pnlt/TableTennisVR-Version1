using System.Collections.Generic;
using Dorkbots.XR.Runtime.Manager;
using UnityEngine;

namespace Dorkbots.XR.Runtime.SoundAndSFX
{
    public abstract class BaseAudio : MonoBehaviour
    {
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected List<Audio> audios;
        
        protected static Dictionary<AudioType, AudioClip> audioClipDict = new Dictionary<AudioType, AudioClip>();

        protected virtual void Awake()
        {
            foreach (var audioClip in audioClipDict)
            {
                AddAudio(audioClip.Key, audioClip.Value);    
            }
        }

        public void AddAudio(AudioType audioType, AudioClip clip)
        {
            audioClipDict.Add(audioType, clip);
        }

        public virtual void PlayClip(AudioType audioType)
        {
            if (audioClipDict.TryGetValue(audioType, out AudioClip clip))
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}