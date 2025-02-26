using _Project.Scripts.Runtime.Enum;
using UnityEngine;
using VadimskyiLab.Events;

namespace Dorkbots.XR.Runtime
{
    
    public class PlayMusicEvent : EventBase<PlaySFXEvent, PlayMusicEventData>
    { }

    public readonly struct PlayMusicEventData
    {
        public readonly SoundTypes audioType;

        public PlayMusicEventData(SoundTypes audioType)
        {
            this.audioType = audioType;
        }
    }
    
    public class PlaySFXEvent : EventBase<PlaySFXEvent, PlaySFXEventData>
    { }

    public readonly struct PlaySFXEventData
    {
        public readonly SoundTypes audioType;
        public readonly Vector3 position;

        public PlaySFXEventData(SoundTypes audioType, Vector3 position = default(Vector3))
        {
            this.audioType = audioType;
            this.position = position;
        }
    }
    
    public class ResetConditionEvent : EventBase<ResetConditionEvent>
    {}
}