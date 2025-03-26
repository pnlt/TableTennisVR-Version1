using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTimer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    
    private float timer = 0f;
    private double videoLength;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.Play();
    }

    private void Update()
    {

        if (!videoPlayer.isPlaying)
        {
            // TODO - transition back to the play scene   
            SceneManager.LoadSceneAsync(2);
        }
    }
}
