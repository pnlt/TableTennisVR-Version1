using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPathTrigger : MonoBehaviour, IPathTrigger
{
    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }

    public void OnPathTriggered() {
        SceneManager.LoadSceneAsync("Tutorial");
    }
}