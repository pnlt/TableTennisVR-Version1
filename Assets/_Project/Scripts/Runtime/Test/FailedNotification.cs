using UnityEngine;

namespace _Project.Scripts.Tests.Runtime.Test
{
    public class FailedNotification : Notification
    {
        private string _notificationMessage;
        private AudioSource failedSoundSrc;
        private AudioClip failedSound;

        public FailedNotification(Checkpoints checkpoints, string notificationMessage = null, AudioSource failedSoundSrc = null, AudioClip failedSound = null) : base(checkpoints)
        {
            _notificationMessage = notificationMessage;
            this.failedSoundSrc = failedSoundSrc;
            this.failedSound = failedSound;
        }

        public override void ResetLine()
        {
            base.ResetLine();
        }
    }
}