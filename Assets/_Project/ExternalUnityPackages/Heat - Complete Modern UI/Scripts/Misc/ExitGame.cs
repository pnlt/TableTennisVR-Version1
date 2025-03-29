using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.Heat
{
    public class ExitGame : MonoBehaviour
    {
        public void Exit() 
        { 
            //Application.Quit();
            SceneManager.LoadSceneAsync(0);
#if UNITY_EDITOR
            Debug.Log("<b>[Heat UI]</b> Exit function works in builds only.");
#endif
        }
    }
}