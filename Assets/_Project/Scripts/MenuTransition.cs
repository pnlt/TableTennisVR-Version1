using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MenuTransition : MonoBehaviour, IPathTrigger
    {
        public void OnPathTriggered() {
            SceneTransitionManager.singleton.GoToSceneAsync(1);
        }
    }
}