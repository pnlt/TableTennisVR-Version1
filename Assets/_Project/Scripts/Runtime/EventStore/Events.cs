using _Project.Scripts.Runtime.Enum;
using UnityEngine;
using VadimskyiLab.Events;

namespace Dorkbots.XR.Runtime
{
    public class PlayMusicEvent : EventBase<PlaySFXEvent, PlayMusicEventData>
    {
    }

    public readonly struct PlayMusicEventData
    {
        public readonly SoundTypes audioType;

        public PlayMusicEventData(SoundTypes audioType)
        {
            this.audioType = audioType;
        }
    }

    public class PlaySFXEvent : EventBase<PlaySFXEvent, PlaySFXEventData>
    {
    }

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
    {
    }

    public class LineAttainmentEvent : EventBase<LineAttainmentEvent, LineData>
    {
    }

    public class LineData
    {
        public SplineCheckpoint Checkpoint;

        public LineData(SplineCheckpoint checkpoint)
        {
            this.Checkpoint = checkpoint;
        }
    }

    public class DisplayScoreEvent : EventBase<DisplayScoreEvent, ScoreData>
    {
    }

    public struct ScoreData
    {
        private readonly float score;
        public float Score => score;

        public ScoreData(float score)
        {
            this.score = score;
        }
    }

    /*public class GameActivationEvent<T> : EventBase<T, GameActivationData> {}

    public readonly struct GameActivationData
    {
        private readonly GameObject @object;
        private readonly bool flag;

        public GameObject gameObj => @object;
        public bool Flag => flag;

        public GameActivationData(bool flag, GameObject @object)
        {
            this.@object = @object;
            this.flag = flag;
        }
    }*/

    #region GameObjectToggle

    public class ModeAlterationNotificationEvent : EventBase<ModeAlterationNotificationEvent, ModeNotificationData>
    {
    }

    public readonly struct ModeNotificationData
    {
        private readonly bool flag;
        public bool Flag => flag;

        public ModeNotificationData(bool flag)
        {
            this.flag = flag;
        }
    }


    public class TimeNotificationEvent : EventBase<TimeNotificationEvent, TimeNotificationData>
    {
    }

    public readonly struct TimeNotificationData
    {
        private readonly bool flag;
        public bool Flag => flag;

        public TimeNotificationData(bool flag)
        {
            this.flag = flag;
        }
    }

    public class TimerActivationEvent : EventBase<TimerActivationEvent, TimerData>
    {
    }

    public readonly struct TimerData
    {
        private readonly bool flag;
        public bool Flag => flag;

        public TimerData(bool flag)
        {
            this.flag = flag;
        }
    }
    
    public class FailedChallengeNotification : EventBase<FailedChallengeNotification, FailedChallengeNotificationData>
    {}
    
    public readonly struct FailedChallengeNotificationData
    {
        private readonly bool flag;
        public bool Flag => flag;

        public FailedChallengeNotificationData(bool flag)
        {
            this.flag = flag;
        }
    }

    #endregion

    public class DisplayTimerEvent : EventBase<DisplayTimerEvent, DisplayTimerData>
    {
    }

    public readonly struct DisplayTimerData
    {
        private readonly float elapsedTime;

        public float ElapsedTime => elapsedTime;

        public DisplayTimerData(float elapsedTime)
        {
            this.elapsedTime = elapsedTime;
        }
    }
}