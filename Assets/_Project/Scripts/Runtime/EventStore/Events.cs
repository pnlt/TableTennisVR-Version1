using UnityEngine;
using VadimskyiLab.Events;

namespace Dorkbots.XR.Runtime
{
    
    public class PlayMusicEvent : EventBase<PlaySFXEvent, PlayMusicEventData>
    { }

    public readonly struct PlayMusicEventData
    {
        public readonly AudioType audioType;

        public PlayMusicEventData(AudioType audioType)
        {
            this.audioType = audioType;
        }
    }
    
    public class PlaySFXEvent : EventBase<PlaySFXEvent, PlaySFXEventData>
    { }

    public readonly struct PlaySFXEventData
    {
        public readonly AudioType audioType;
        public readonly Vector3 position;

        public PlaySFXEventData(AudioType audioType, Vector3 position = default(Vector3))
        {
            this.audioType = audioType;
            this.position = position;
        }
    }
    
    public class ResetConditionEvent : EventBase<ResetConditionEvent>
    {}
}