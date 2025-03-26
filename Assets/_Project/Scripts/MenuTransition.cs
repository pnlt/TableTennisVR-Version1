using UnityEngine;
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
            SceneTransitionManager.singleton.GoToSceneAsync(1);
        }
    }
}