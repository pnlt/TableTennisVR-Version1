using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Beautify.Universal {

    [CustomEditor(typeof(BeautifyRendererFeature))]
    public class BeautifyRenderFeatureEditor : Editor {

        SerializedProperty renderPassEvent, ignorePostProcessingOption;
#if ENABLE_VR && ENABLE_XR_MODULE
        SerializedProperty clearXRColorBuffer;
#endif
        SerializedProperty cameraLayerMask;
        SerializedProperty stripBeautifyTonemappingACES, stripBeautifyTonemappingACESFitted, stripBeautifyTonemappingAGX;
        SerializedProperty stripBeautifySharpen, stripBeautifyDithering, stripBeautifyEdgeAA;
        SerializedProperty stripBeautifyLUT, stripBeautifyLUT3D, stripBeautifyColorTweaks;
        SerializedProperty stripBeautifyBloom, stripBeautifyLensDirt, stripBeautifyChromaticAberration;
        SerializedProperty stripBeautifyDoF, stripBeautifyDoFTransparentSupport;
        SerializedProperty stripBeautifyEyeAdaptation, stripBeautifyPurkinje;
        SerializedProperty stripBeautifyVignetting, stripBeautifyOutline;
        SerializedProperty stripBeautifyNightVision, stripBeautifyThermalVision;
        SerializedProperty stripBeautifyFrame;
        SerializedProperty stripUnityFilmGrain, stripUnityDithering, stripUnityTonemapping;
        SerializedProperty stripUnityBloom, stripUnityChromaticAberration;
        SerializedProperty stripUnityDistortion, stripUnityDebugVariants;

        void OnEnable () {
            renderPassEvent = serializedObject.FindProperty("renderPassEvent");
            ignorePostProcessingOption = serializedObject.FindProperty("ignorePostProcessingOption");
#if ENABLE_VR && ENABLE_XR_MODULE
            clearXRColorBuffer = serializedObject.FindProperty("clearXRColorBuffer");
#endif
            cameraLayerMask = serializedObject.FindProperty("cameraLayerMask");
            stripBeautifyTonemappingACES = serializedObject.FindProperty("stripBeautifyTonemappingACES");
            stripBeautifyTonemappingACESFitted = serializedObject.FindProperty("stripBeautifyTonemappingACESFitted");
            stripBeautifyTonemappingAGX = serializedObject.FindProperty("stripBeautifyTonemappingAGX");
            stripBeautifySharpen = serializedObject.FindProperty("stripBeautifySharpen");
            stripBeautifyDithering = serializedObject.FindProperty("stripBeautifyDithering");
            stripBeautifyEdgeAA = serializedObject.FindProperty("stripBeautifyEdgeAA");
            stripBeautifyLUT = serializedObject.FindProperty("stripBeautifyLUT");
            stripBeautifyLUT3D = serializedObject.FindProperty("stripBeautifyLUT3D");
            stripBeautifyColorTweaks = serializedObject.FindProperty("stripBeautifyColorTweaks");
            stripBeautifyBloom = serializedObject.FindProperty("stripBeautifyBloom");
            stripBeautifyLensDirt = serializedObject.FindProperty("stripBeautifyLensDirt");
            stripBeautifyChromaticAberration = serializedObject.FindProperty("stripBeautifyChromaticAberration");
            stripBeautifyDoF = serializedObject.FindProperty("stripBeautifyDoF");
            stripBeautifyDoFTransparentSupport = serializedObject.FindProperty("stripBeautifyDoFTransparentSupport");
            stripBeautifyEyeAdaptation = serializedObject.FindProperty("stripBeautifyEyeAdaptation");
            stripBeautifyPurkinje = serializedObject.FindProperty("stripBeautifyPurkinje");
            stripBeautifyVignetting = serializedObject.FindProperty("stripBeautifyVignetting");
            stripBeautifyOutline = serializedObject.FindProperty("stripBeautifyOutline");
            stripBeautifyNightVision = serializedObject.FindProperty("stripBeautifyNightVision");
            stripBeautifyThermalVision = serializedObject.FindProperty("stripBeautifyThermalVision");
            stripBeautifyFrame = serializedObject.FindProperty("stripBeautifyFrame");
            stripUnityFilmGrain = serializedObject.FindProperty("stripUnityFilmGrain");
            stripUnityDithering = serializedObject.FindProperty("stripUnityDithering");
            stripUnityTonemapping = serializedObject.FindProperty("stripUnityTonemapping");
            stripUnityBloom = serializedObject.FindProperty("stripUnityBloom");
            stripUnityChromaticAberration = serializedObject.FindProperty("stripUnityChromaticAberration");
            stripUnityDistortion = serializedObject.FindProperty("stripUnityDistortion");
            stripUnityDebugVariants = serializedObject.FindProperty("stripUnityDebugVariants");
        }

        public override void OnInspectorGUI () {

            serializedObject.Update();

            EditorGUILayout.PropertyField(renderPassEvent);
            EditorGUILayout.PropertyField(ignorePostProcessingOption);
#if ENABLE_VR && ENABLE_XR_MODULE
            EditorGUILayout.PropertyField(clearXRColorBuffer);
#endif
            EditorGUILayout.PropertyField(cameraLayerMask);

            GUIStyle boxStyle = new GUIStyle("box");
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            boxStyle.margin = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUILayout.LabelField("Beautify Shader Features Stripping", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Optimize shader compilation time by stripping unused Beautify features. Select the features you wish to exclude from the build.", MessageType.Info);

            void DrawStripToggle (SerializedProperty property, string label) {
                EditorGUILayout.BeginHorizontal();
                property.boolValue = GUILayout.Toggle(property.boolValue, "", GUILayout.Width(20));
                GUILayout.Label(label);
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Autoselect Unused Beautify Features", EditorStyles.miniButton)) {
                if (EditorUtility.DisplayDialog("Autoselect Unused Beautify Features", "This will disable features not used in the current scene. Do you want to proceed?", "Yes", "No")) {
                    
                    #if UNITY_2023_1_OR_NEWER
                        var beautifyVolumes = FindObjectsByType<Beautify>(FindObjectsSortMode.None);
                    #else
                        var beautifyVolumes = FindObjectsOfType<Beautify>();
                    #endif

                    bool isFeatureUsed (System.Func<Beautify, bool> predicate) {
                        foreach (var volume in beautifyVolumes) {
                            if (predicate(volume)) {
                                return true;
                            }
                        }
                        return false;
                    }

                    stripBeautifySharpen.boolValue = !isFeatureUsed(b => b.sharpenIntensity.value > 0f);
                    stripBeautifyDithering.boolValue = !isFeatureUsed(b => b.ditherIntensity.value > 0f);
                    stripBeautifyEdgeAA.boolValue = !isFeatureUsed(b => b.antialiasStrength.value > 0f);
                    stripBeautifyTonemappingACES.boolValue = !isFeatureUsed(b => b.tonemap.value == Beautify.TonemapOperator.ACES);
                    stripBeautifyTonemappingACESFitted.boolValue = !isFeatureUsed(b => b.tonemap.value == Beautify.TonemapOperator.ACESFitted);
                    stripBeautifyTonemappingAGX.boolValue = !isFeatureUsed(b => b.tonemap.value == Beautify.TonemapOperator.AGX);
                    stripBeautifyLUT.boolValue = !isFeatureUsed(b => b.lut.value && b.lutIntensity.value > 0 && b.lutTexture.value != null && !(b.lutTexture.value is Texture3D));
                    stripBeautifyLUT3D.boolValue = !isFeatureUsed(b => b.lut.value && b.lutIntensity.value > 0 && b.lutTexture.value is Texture3D);
                    stripBeautifyColorTweaks.boolValue = !isFeatureUsed(b => b.sepia.value > 0 || b.daltonize.value > 0 || b.colorTempBlend.value > 0);
                    stripBeautifyBloom.boolValue = !isFeatureUsed(b => b.bloomIntensity.value > 0f);
                    stripBeautifyLensDirt.boolValue = !isFeatureUsed(b => b.lensDirtIntensity.value > 0);
                    stripBeautifyChromaticAberration.boolValue = !isFeatureUsed(b => b.chromaticAberrationIntensity.value > 0f);
                    stripBeautifyDoF.boolValue = !isFeatureUsed(b => b.depthOfField.value);
                    stripBeautifyDoFTransparentSupport.boolValue = !isFeatureUsed(b => b.depthOfFieldTransparentSupport.value);
                    stripBeautifyEyeAdaptation.boolValue = !isFeatureUsed(b => b.eyeAdaptation.value);
                    stripBeautifyPurkinje.boolValue = !isFeatureUsed(b => b.purkinje.value);
                    stripBeautifyVignetting.boolValue = !isFeatureUsed(b => b.vignettingOuterRing.value > 0f);
                    stripBeautifyOutline.boolValue = !isFeatureUsed(b => b.outline.value);
                    stripBeautifyNightVision.boolValue = !isFeatureUsed(b => b.nightVision.value);
                    stripBeautifyThermalVision.boolValue = !isFeatureUsed(b => b.thermalVision.value);
                    stripBeautifyFrame.boolValue = !isFeatureUsed(b => b.frame.value);
                }
            }

            // Image Enhancement section
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Image Enhancement", EditorStyles.boldLabel);
            if (GUILayout.Button("Toggle All", EditorStyles.miniButton, GUILayout.Width(80))) {
                bool allStripped = stripBeautifySharpen.boolValue &&
                                 stripBeautifyDithering.boolValue &&
                                 stripBeautifyEdgeAA.boolValue;
                stripBeautifySharpen.boolValue = !allStripped;
                stripBeautifyDithering.boolValue = !allStripped;
                stripBeautifyEdgeAA.boolValue = !allStripped;
            }
            EditorGUILayout.EndHorizontal();
            DrawStripToggle(stripBeautifySharpen, "Strip Sharpen");
            DrawStripToggle(stripBeautifyDithering, "Strip Dithering");
            DrawStripToggle(stripBeautifyEdgeAA, "Strip Edge AA");

            // Tonemapping section
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tonemapping", EditorStyles.boldLabel);
            if (GUILayout.Button("Toggle All", EditorStyles.miniButton, GUILayout.Width(80))) {
                bool allStripped = stripBeautifyTonemappingACES.boolValue &&
                                 stripBeautifyTonemappingACESFitted.boolValue &&
                                 stripBeautifyTonemappingAGX.boolValue;
                stripBeautifyTonemappingACES.boolValue = !allStripped;
                stripBeautifyTonemappingACESFitted.boolValue = !allStripped;
                stripBeautifyTonemappingAGX.boolValue = !allStripped;
            }
            EditorGUILayout.EndHorizontal();
            DrawStripToggle(stripBeautifyTonemappingACES, "Strip ACES Tonemapping");
            DrawStripToggle(stripBeautifyTonemappingACESFitted, "Strip ACES Fitted Tonemapping");
            DrawStripToggle(stripBeautifyTonemappingAGX, "Strip AGX Tonemapping");

            // Color Grading section
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color Grading", EditorStyles.boldLabel);
            if (GUILayout.Button("Toggle All", EditorStyles.miniButton, GUILayout.Width(80))) {
                bool allStripped = stripBeautifyLUT.boolValue &&
                                 stripBeautifyLUT3D.boolValue &&
                                 stripBeautifyColorTweaks.boolValue;
                stripBeautifyLUT.boolValue = !allStripped;
                stripBeautifyLUT3D.boolValue = !allStripped;
                stripBeautifyColorTweaks.boolValue = !allStripped;
            }
            EditorGUILayout.EndHorizontal();
            DrawStripToggle(stripBeautifyLUT, "Strip LUT");
            DrawStripToggle(stripBeautifyLUT3D, "Strip LUT 3D");
            DrawStripToggle(stripBeautifyColorTweaks, new GUIContent("Strip Color Tweaks", "Refers to sepia, daltonize and color temperature").text);

            // Effects section
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
            if (GUILayout.Button("Toggle All", EditorStyles.miniButton, GUILayout.Width(80))) {
                bool allStripped = stripBeautifyBloom.boolValue &&
                                 stripBeautifyLensDirt.boolValue &&
                                 stripBeautifyChromaticAberration.boolValue &&
                                 stripBeautifyDoF.boolValue &&
                                 stripBeautifyDoFTransparentSupport.boolValue &&
                                 stripBeautifyEyeAdaptation.boolValue &&
                                 stripBeautifyPurkinje.boolValue &&
                                 stripBeautifyVignetting.boolValue &&
                                 stripBeautifyOutline.boolValue &&
                                 stripBeautifyNightVision.boolValue &&
                                 stripBeautifyThermalVision.boolValue &&
                                 stripBeautifyFrame.boolValue;
                stripBeautifyBloom.boolValue = !allStripped;
                stripBeautifyLensDirt.boolValue = !allStripped;
                stripBeautifyChromaticAberration.boolValue = !allStripped;
                stripBeautifyDoF.boolValue = !allStripped;
                stripBeautifyDoFTransparentSupport.boolValue = !allStripped;
                stripBeautifyEyeAdaptation.boolValue = !allStripped;
                stripBeautifyPurkinje.boolValue = !allStripped;
                stripBeautifyVignetting.boolValue = !allStripped;
                stripBeautifyOutline.boolValue = !allStripped;
                stripBeautifyNightVision.boolValue = !allStripped;
                stripBeautifyThermalVision.boolValue = !allStripped;
                stripBeautifyFrame.boolValue = !allStripped;
            }
            EditorGUILayout.EndHorizontal();
            DrawStripToggle(stripBeautifyBloom, "Strip Bloom, Anamorphic & Sun Flares");
            DrawStripToggle(stripBeautifyLensDirt, "Strip Lens Dirt");
            DrawStripToggle(stripBeautifyChromaticAberration, "Strip Chromatic Aberration");
            DrawStripToggle(stripBeautifyDoF, "Strip Depth of Field");
            DrawStripToggle(stripBeautifyDoFTransparentSupport, "Strip DoF Transparent Support");
            DrawStripToggle(stripBeautifyEyeAdaptation, "Strip Eye Adaptation");
            DrawStripToggle(stripBeautifyPurkinje, "Strip Purkinje");
            DrawStripToggle(stripBeautifyVignetting, "Strip Vignetting");
            DrawStripToggle(stripBeautifyOutline, "Strip Outline");
            DrawStripToggle(stripBeautifyNightVision, "Strip Night Vision");
            DrawStripToggle(stripBeautifyThermalVision, "Strip Thermal Vision");
            DrawStripToggle(stripBeautifyFrame, "Strip Frame");

            EditorGUILayout.Separator();
            // Unity Post Processing section
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Unity Post Processing Stripping", EditorStyles.boldLabel);
            if (GUILayout.Button("Toggle All", EditorStyles.miniButton, GUILayout.Width(80))) {
                bool allStripped = stripUnityFilmGrain.boolValue &&
                                 stripUnityDithering.boolValue &&
                                 stripUnityTonemapping.boolValue &&
                                 stripUnityBloom.boolValue &&
                                 stripUnityChromaticAberration.boolValue &&
                                 stripUnityDistortion.boolValue &&
                                 stripUnityDebugVariants.boolValue;
                stripUnityFilmGrain.boolValue = !allStripped;
                stripUnityDithering.boolValue = !allStripped;
                stripUnityTonemapping.boolValue = !allStripped;
                stripUnityBloom.boolValue = !allStripped;
                stripUnityChromaticAberration.boolValue = !allStripped;
                stripUnityDistortion.boolValue = !allStripped;
                stripUnityDebugVariants.boolValue = !allStripped;
            }
            EditorGUILayout.EndHorizontal();
            DrawStripToggle(stripUnityFilmGrain, "Strip Film Grain");
            DrawStripToggle(stripUnityDithering, "Strip Dithering");
            DrawStripToggle(stripUnityTonemapping, "Strip Tonemapping");
            DrawStripToggle(stripUnityBloom, "Strip Bloom");
            DrawStripToggle(stripUnityChromaticAberration, "Strip Chromatic Aberration");
            DrawStripToggle(stripUnityDistortion, "Strip Distortion");
            DrawStripToggle(stripUnityDebugVariants, "Strip Debug Variants");
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Select Beautify Volume >")) {
#if UNITY_2023_1_OR_NEWER
                var volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);
#else           
                var volumes = FindObjectsOfType<Volume>();
#endif

                foreach (var volume in volumes) {
                    if (volume.sharedProfile != null && volume.sharedProfile.TryGet<Beautify>(out var beautify)) {
                        Selection.activeObject = volume.gameObject;
                        EditorGUIUtility.PingObject(volume.gameObject);
                        break;
                    }
                }
            }

            if (serializedObject.ApplyModifiedProperties()) {
                BeautifyRendererFeature feature = (BeautifyRendererFeature)target;
                feature.SetStripShaderKeywords();
            }
        }
    }
}