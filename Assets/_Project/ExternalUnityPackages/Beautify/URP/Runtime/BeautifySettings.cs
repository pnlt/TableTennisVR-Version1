using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Beautify.Universal {

    public delegate float OnBeforeFocusEvent(float currentFocusDistance);


    [ExecuteAlways]
    public class BeautifySettings : MonoBehaviour {

        [Header("Scene Settings")]
        public Transform sun;
        public Transform depthOfFieldTarget;

        public OnBeforeFocusEvent OnBeforeFocus;

        public static float depthOfFieldCurrentFocalPointDistance;

        public static int bloomExcludeMask;

        public static int anamorphicFlaresExcludeMask;

        public static bool dofTransparentSupport;

        public static int dofTransparentLayerMask;

        public static bool dofTransparentDoubleSided;

        public static bool dofAlphaTestSupport;

        public static int dofAlphaTestLayerMask;

        public static bool dofAlphaTestDoubleSided;

        public static bool outlineDepthPrepass;
        public static bool outlineUseObjectId;
        public static int outlineLayerMask;
        public static float outlineLayerCutOff;

        public static bool outlineDepthPrepassUseOptimizedShader;

        internal static bool _refreshAlphaClipRenderers;

        static BeautifySettings _instance;
        static Volume _beautifyVolume;
        static Beautify _beautify;

        /// <summary>
        /// Forces a reset of the internal cached settings of Beautify. Call this method if Beautify settings are not resetted when switching scenes.
        /// </summary>
        public static void UnloadBeautify() {
            _instance = null;
            _beautifyVolume = null;
            _beautify = null;
        }

        /// <summary>
        /// Returns a reference to the Beautify Settings component attached to the Post Processing Layer or camera
        /// </summary>
        /// <value>The instance.</value>
        public static BeautifySettings instance {
            get {
                if (_instance == null) {
#if UNITY_2023_1_OR_NEWER
                    _instance = FindAnyObjectByType<BeautifySettings>();
#else
                    _instance = FindObjectOfType<BeautifySettings>();
#endif
                    if (_instance == null) {
                        // Check if there's a single volume component, then add BeautifySettings singleton to that gameobject
                        _beautifyVolume = FindBeautifyVolume();
                        if (_beautifyVolume == null) {
                            return null;
                        }
                        GameObject go = _beautifyVolume.gameObject;
                        _instance = go.GetComponent<BeautifySettings>();
                        if (_instance == null) {
                            _instance = go.AddComponent<BeautifySettings>();
                        }
                    }
                }
                return _instance;
            }
        }


        static Volume FindBeautifyVolume() {
#if UNITY_2023_1_OR_NEWER
            Volume[] vols = FindObjectsByType<Volume>(FindObjectsSortMode.None);
#else
            Volume[] vols = FindObjectsOfType<Volume>();
#endif
            foreach (Volume volume in vols) {
                if (volume.sharedProfile != null && volume.sharedProfile.Has<Beautify>()) {
                    _beautifyVolume = volume;
                    return volume;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a reference to the settings of Beautify in the Post Processing Profile
        /// </summary>
        /// <value>The shared settings.</value>
        public static Beautify sharedSettings {
            get {
                if (_beautify != null) return _beautify;
                if (_beautifyVolume == null) FindBeautifyVolume();
                if (_beautifyVolume == null) return null;

                bool foundEffectSettings = _beautifyVolume.sharedProfile.TryGet(out _beautify);
                if (!foundEffectSettings) {
                    Debug.Log("Cant load Beautify settings");
                    return null;
                }
                return _beautify;
            }
        }

        /// <summary>
        /// Returns the profile of the volume in the scene where Beautify has been found
        /// </summary>
        public static VolumeProfile currentProfile {
            get {
                if (_beautifyVolume == null) {
                    FindBeautifyVolume();
                }
                if (_beautifyVolume == null) return null;
                return _beautifyVolume.sharedProfile;
            }
        }

        /// <summary>
        /// Returns a copy of the settings of Beautify in the Post Processing Profile
        /// </summary>
        /// <value>The settings.</value>
        public static Beautify settings {
            get {
                if (_beautify != null) return _beautify;
                if (_beautifyVolume == null) FindBeautifyVolume();
                if (_beautifyVolume == null) return null;

                bool foundEffectSettings = _beautifyVolume.profile.TryGet(out _beautify);
                if (!foundEffectSettings) {
                    Debug.Log("Cant load Beautify settings");
                    return null;
                }
                return _beautify;
            }
        }


        public static void Blink(float duration, float maxValue = 1) {
            if (duration <= 0)
                return;
            BeautifySettings i = instance;
            if (i == null) return;
            i.StartCoroutine(i.DoBlink(duration, maxValue));
        }

        IEnumerator DoBlink(float duration, float maxValue) {

            Beautify beautify = settings;
            if (beautify == null) yield break;
            float start = Time.time;
            WaitForEndOfFrame w = new WaitForEndOfFrame();
            beautify.vignettingBlink.overrideState = true;
            float t;
            // Close
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeOut = t * (2f - t);
                beautify.vignettingBlink.value = easeOut * maxValue;
                yield return w;
            } while (t < 1f);

            // Open
            start = Time.time;
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeIn = t * t;
                beautify.vignettingBlink.value = (1f - easeIn) * maxValue;
                yield return w;
            } while (t < 1f);
            beautify.vignettingBlink.overrideState = false;
        }

        void OnEnable() {
            #if UNITY_EDITOR
            if (!Application.isPlaying) {
                BeautifyRendererFeature.StripBeautifyFeatures();
            }
            #endif
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
            UnloadBeautify();
        }


        /// <summary>
        /// Updates the list of alpha clip renderers
        /// </summary>
        public static void UpdateAlphaClipRenderers() {
            _refreshAlphaClipRenderers = true;
        }

    }
}
