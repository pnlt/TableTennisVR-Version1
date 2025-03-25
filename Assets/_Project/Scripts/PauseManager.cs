using UnityEngine;

public class PauseManager : MonoBehaviour, IPathTrigger
{
    private void Pause() {
        Time.timeScale = 0f;
    }

    public void OnPathTriggered() {
        Pause();
    }
}