using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.Heat
{
    [DisallowMultipleComponent]
    public class MenuManager : MonoBehaviour
    {
        // Resources
        public UIManager UIManagerAsset;
        public Animator splashScreen;
        [SerializeField] private GameObject mainContent;
        [SerializeField] private ImageFading initPanel;

        // Helpers
        float splashInTime;
        float splashOutTime;
        public static bool bypassSplashScreen;

        void Awake()
        {
            Time.timeScale = 1;

            if (initPanel != null) { initPanel.gameObject.SetActive(true); }
            if (splashScreen != null) { splashScreen.gameObject.SetActive(false); }
        }

        void Start()
        {
            StartCoroutine("StartInitialize");
        }

        public void DisableSplashScreen() 
        {
            StopCoroutine("DisableSplashScreenAnimator");
            StartCoroutine("FinalizeSplashScreen");

            splashScreen.enabled = true;
            splashScreen.Play("Out");
        }

        void Initialize()
        {
            if (UIManagerAsset == null || mainContent == null)
            {
                Debug.LogError("<b>[Heat UI]</b> Cannot initialize the resources due to missing resources.", this);
                return;
            }

            mainContent.gameObject.SetActive(false);

            if (!bypassSplashScreen && UIManagerAsset.enableSplashScreen)
            {
                if (splashScreen == null)
                {
                    Debug.LogError("<b>[Heat UI]</b> Splash Screen is enabled but its resource is missing. Please assign the correct variable for 'Splash Screen'.", this);
                    return;
                }

                // Getting in and out animation length
                AnimationClip[] clips = splashScreen.runtimeAnimatorController.animationClips;
                splashInTime = clips[0].length;
                splashOutTime = clips[1].length;

                splashScreen.enabled = true;
                splashScreen.gameObject.SetActive(true);
                StartCoroutine("DisableSplashScreenAnimator");

                if (UIManagerAsset.showSplashScreenOnce)
                {
                    bypassSplashScreen = true;
                }
            }

            else
            {
                if (mainContent == null)
                {
                    Debug.LogError("<b>[Heat UI]</b> 'Main Panels' is missing. Please assign the correct variable for 'Main Panels'.", this);
                    return;
                }

                if (splashScreen != null) { splashScreen.gameObject.SetActive(false); }
                mainContent.gameObject.SetActive(false);
                StartCoroutine("FinalizeSplashScreen");
            }
        }

        IEnumerator StartInitialize()
        {
            yield return new WaitForSeconds(0.5f);
            if (initPanel != null) { initPanel.FadeOut(); }
            Initialize();
        }

        IEnumerator DisableSplashScreenAnimator()
        {
            yield return new WaitForSeconds(splashInTime + 0.1f);
            splashScreen.enabled = false;
        }

        IEnumerator FinalizeSplashScreen()
        {
            yield return new WaitForSeconds(splashOutTime + 0.1f);
           
            if (UIManagerAsset != null && UIManagerAsset.enableSplashScreen) 
            {
                splashScreen.gameObject.SetActive(false); 
            }

            mainContent.gameObject.SetActive(true);

            if (ControllerManager.instance != null
                && ControllerManager.instance.gamepadEnabled
                && ControllerManager.instance.firstSelected != null
                && ControllerManager.instance.firstSelected.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(ControllerManager.instance.firstSelected);
            }
        }
    }
}