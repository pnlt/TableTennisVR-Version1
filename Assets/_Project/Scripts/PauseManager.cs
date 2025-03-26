using UnityEngine;

public class PauseManager : MonoBehaviour, IPathTrigger
{
    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }

    private void Pause() {
        Time.timeScale = 0f;
    }


    public void OnPathTriggered() {
        Pause();
    }
}