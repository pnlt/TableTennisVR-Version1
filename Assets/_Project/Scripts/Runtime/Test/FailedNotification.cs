using UnityEngine;

namespace _Project.Scripts.Tests.Runtime.Test
{
    public class FailedNotification : Notification
    {
        private string _notificationMessage;
        private AudioSource failedSoundSrc;
        private AudioClip failedSound;

        public FailedNotification(Checkpoints checkpoints, string notificationMessage = null,
            AudioSource failedSoundSrc = null, AudioClip failedSound = null) : base(checkpoints)
        {
            _notificationMessage = notificationMessage;
            this.failedSoundSrc = failedSoundSrc;
            this.failedSound = failedSound;
        }

        public override void ResetLine()
        {
            PlayFailedSound();
            base.ResetLine();
            DisplayFailedNotification();
        }

        private void PlayFailedSound()
        {
            // Play sound when player was failed to score
        }

        private void DisplayFailedNotification()
        {
            // Notify player has not scored - Display UI or effects
        }
    }
}