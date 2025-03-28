using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RoundedBoxVideoController : MonoBehaviour
{
    public Slider timeSlider;
    public Slider volumeSlider;
    public float animationDuration;
    public float animationTime;
    public int cycleCount;
    public VideoPlayer videoPlayer;

    public Sprite playIcon;
    public Sprite pauseIcon;
    public Image playPauseImg;

    public bool isPlaying;

    private float animationCycleDuration;

    [Header("Time Labels")] public TextMeshProUGUI leftLabel;
    public TextMeshProUGUI rightLabel;

    [Header("Background Material Settings")]
    public Image backgroundImage;

    public Vector2 direction;
    public Color colorA;
    public Color colorB;

    private readonly int columnDirectionID = Shader.PropertyToID("columnDirection");
    private readonly int rowDirectionID = Shader.PropertyToID("rowDirection");
    private readonly int animationTimeID = Shader.PropertyToID("animationTime");
    private readonly int colorAID = Shader.PropertyToID("colorA");
    private readonly int colorBID = Shader.PropertyToID("colorB");

    private struct BoxAnimation
    {
        public RectTransform rectTransform;
        public Image image;
        public float duration;
        public float startHeight;
        public float animationMaxHeight;
        public float startVelocity;
        public float startTime;
        public float acceleration;

        public void Update(float animationTime)
        {
            var animTime = animationTime - startTime;
            animTime = Mathf.Clamp(animTime, 0.0f, duration);
            var animTime2 = animTime * animTime;

            var parabolicHeight = (startVelocity * animTime) - (0.5f * acceleration * animTime2);
            var heightParam = parabolicHeight / animationMaxHeight;
            var currentHeight = startHeight - parabolicHeight;
            var position = rectTransform.anchoredPosition;
            position.y = currentHeight;
            rectTransform.anchoredPosition = position;

            rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, heightParam * 360.0f);
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }

    private List<BoxAnimation> animations;

    private void OnEnable()
    {
        UpdateBackgroundMaterialProperties();
    }

    private void Start()
    {
        timeSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeValueChange(); });

        SetPlay();
        UpdateBackgroundMaterialProperties();
    }

    private void OnVolumeValueChange()
    {
        videoPlayer.SetDirectAudioVolume(0, volumeSlider.value);
    }

    private void UpdateBackgroundMaterialProperties()
    {
        var normalizedDirection = direction.normalized;
        backgroundImage.materialForRendering.SetVector(columnDirectionID, normalizedDirection);
        backgroundImage.materialForRendering.SetVector(rowDirectionID,
            new Vector2(-normalizedDirection.y, normalizedDirection.x));

        backgroundImage.materialForRendering.SetColor(colorAID, colorA.linear);
        backgroundImage.materialForRendering.SetColor(colorBID, colorB.linear);

        backgroundImage.materialForRendering.SetFloat(animationTimeID, animationTime);
    }

    private void OnSliderValueChange()
    {
        animationTime = timeSlider.value * animationDuration;
        videoPlayer.time = animationTime;
    }

    public void GoBackTenSeconds()
    {
        if (animationTime - 10 >= 0)
        {
            animationTime -= 10;
            timeSlider.value = animationTime / animationDuration;
        }
    }

    public void GoAheadTenSeconds()
    {
        if (animationTime + 10 < animationDuration)
        {
            animationTime += 10;
            timeSlider.value = animationTime / animationDuration;
        }
    }

    public void TogglePlayPause()
    {
        if (isPlaying)
        {
            SetPaused();
        }
        else
        {
            if (Mathf.Abs(animationDuration - animationTime) < 0.1f)
            {
                animationTime = 0.0f;
            }

            SetPlay();
        }
    }

    private void SetPaused()
    {
        isPlaying = false;
        videoPlayer.Pause();
        playPauseImg.sprite = playIcon;
    }

    private void SetPlay()
    {
        isPlaying = true;
        videoPlayer.Play();
        playPauseImg.sprite = pauseIcon;
    }

    private string FormatTime(float seconds)
    {
        var mins = seconds / 60.0f;
        var secs = (mins - Mathf.Floor(mins)) * 60.0f;
        mins = Mathf.Floor(mins);

        var iMins = (int)mins;
        var iSecs = (int)secs;

        var secsFormat = iSecs < 10 ? $"0{iSecs}" : $"{iSecs}";
        return $"{iMins}:{secsFormat}";
    }

    private void LateUpdate()
    {
        if (isPlaying)
        {
            animationTime += Time.deltaTime;
            timeSlider.SetValueWithoutNotify(animationTime / animationDuration);
            if (animationTime > animationDuration)
            {
                animationTime = animationDuration;
                SetPaused();
            }
        }
        else
        {
            animationTime = timeSlider.value * animationDuration;
        }

        //Time labels
        var remainingTime = Mathf.Round(animationDuration - animationTime);

        leftLabel.SetText(FormatTime(animationTime));
        rightLabel.SetText(FormatTime(remainingTime));

        //Animated background
        backgroundImage.materialForRendering.SetFloat(animationTimeID, animationTime);
    }
}