using System;
using _Project.Scripts.Runtime.Enum;
using UnityEngine;
using VadimskyiLab.Events;

namespace Dorkbots.XR.Runtime.Manager
{
    [Serializable]
    public class Audio
    {
        public string name;
        public SoundTypes soundTypes;
        public AudioClip audioClip;
    }
    
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource audioSource;
        public MusicAudio musicAudio;
        public SFX_Audio sfxAudio;
        
        protected override void Awake()
        {
            base.Awake();
            ReferenceComponents();
        }

        private void OnEnable()
        {
            PlayMusicEvent.Subscribe(PlayMusicAudioClip);
            PlaySFXEvent.Subscribe(PlaySoundEffect);
        }

        private void ReferenceComponents()
        {
            audioSource = GetComponent<AudioSource>();
            musicAudio = GetComponentInChildren<MusicAudio>();    
            sfxAudio = GetComponentInChildren<SFX_Audio>();
        }

        #region Sound actions

        private void PlayMusicAudioClip(PlayMusicEventData playMusicEventData)
        {
            musicAudio.PlayClip(playMusicEventData.audioType);
        }

        private void PlaySoundEffect(PlaySFXEventData sfxEvent)
        {
            if (sfxEvent.position == default(Vector3))
                sfxAudio.PlayClip(sfxEvent.audioType);
            else
                sfxAudio.PlayClip(sfxEvent.audioType, sfxEvent.position);
            
        }

        #endregion

        private void OnDisable()
        {
            PlayMusicEvent.Unsubscribe(PlayMusicAudioClip);
            PlaySFXEvent.Unsubscribe(PlaySoundEffect);
        }
    }
    
}