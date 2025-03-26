using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MenuTransition : MonoBehaviour, IPathTrigger
    {
        [SerializeField] private bool isEnabled = true;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public void OnPathTriggered() {
            SceneManager.LoadSceneAsync(1);
            //SceneTransitionManager.singleton.GoToSceneAsync(1);
        }
    }
}