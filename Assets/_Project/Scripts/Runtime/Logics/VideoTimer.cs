using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTimer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private float timer = 0f;
    private double videoLength;

    private void Start()
    {
        //videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoLength = videoPlayer.clip.length;
    }

    private void Update()
    {
        //CountdownTimer();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            videoPlayer.time = 8;
    }

    private void CountdownTimer()
    {
        if (videoPlayer)
        {
            timer += Time.deltaTime;
            if (timer >= videoLength)
            {
                // TODO - Transition to new scene
            }
        }
    }
}