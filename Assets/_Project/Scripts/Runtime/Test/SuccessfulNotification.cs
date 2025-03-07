using _Project.Scripts.Tests.Runtime.Test;
using UnityEngine;

public class SuccessfulNotification : Notification
{
    private string _notifiedMessage;
    private AudioSource successfulSoundSrc;
    private AudioClip successfulSound;
    

    public SuccessfulNotification(Checkpoints checkpoints, string notifiedMessage = "", AudioSource successfulSoundSrc = null, AudioClip successfulSound = null) : base(checkpoints)
    {
        _notifiedMessage = notifiedMessage;
        this.successfulSoundSrc = successfulSoundSrc;
        this.successfulSound = successfulSound;
    }

    public override void ResetLine()
    {
        PlaySuccessfulSound();
        base.ResetLine();
        DisplaySuccessfulNotification();
    }

    private void PlaySuccessfulSound()
    {
        // Play sound when player successfully scored
    }

    private void DisplaySuccessfulNotification()
    {
        // Notify player has scored - Display UI or effects
    }
}
