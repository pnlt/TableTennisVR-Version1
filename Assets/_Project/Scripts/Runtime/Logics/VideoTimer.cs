using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTimer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private float timer = 0f;
    private double videoLength;

    private void Awake() {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start() {
        var totalFrame = videoPlayer.clip.frameCount;
        var frameRate = videoPlayer.clip.frameRate;
        videoLength = totalFrame / frameRate;

        videoPlayer.Play();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= videoLength)
        {
            // TODO - transition back to the play scene   
            SceneManager.LoadSceneAsync(3);
        }
    }
}