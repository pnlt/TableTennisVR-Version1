#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    [CustomEditor(typeof(ModeSelector))]
    public class ModeSelectorEditor : Editor
    {
        private GUISkin customSkin;
        private ModeSelector msTarget;
        private int currentTab;

        private void OnEnable()
        {
            msTarget = (ModeSelector)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HeatUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = HeatUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            HeatUIEditorHandler.DrawComponentHeader(customSkin, "Horizontal Selector Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = HeatUIEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var currentModeIndex = serializedObject.FindProperty("currentModeIndex");
            var items = serializedObject.FindProperty("items");
            var headerTitle = serializedObject.FindProperty("headerTitle");
            var headerTitleKey = serializedObject.FindProperty("headerTitleKey");

            var onClick = serializedObject.FindProperty("onClick");

            var modeSelectPopup = serializedObject.FindProperty("modeSelectPopup");
            var transitionPanel = serializedObject.FindProperty("transitionPanel");
            var itemParent = serializedObject.FindProperty("itemParent");
            var itemPreset = serializedObject.FindProperty("itemPreset");
            var normalCG = serializedObject.FindProperty("normalCG");
            var highlightCG = serializedObject.FindProperty("highlightCG");
            var disabledCG = serializedObject.FindProperty("disabledCG");
            var disabledHeaderObj = serializedObject.FindProperty("disabledHeaderObj");
            var normalHeaderObj = serializedObject.FindProperty("normalHeaderObj");
            var highlightHeaderObj = serializedObject.FindProperty("highlightHeaderObj");
            var disabledTextObj = serializedObject.FindProperty("disabledTextObj");
            var normalTextObj = serializedObject.FindProperty("normalTextObj");
            var highlightTextObj = serializedObject.FindProperty("highlightTextObj");
            var backgroundImage = serializedObject.FindProperty("backgroundImage");
            var disabledIconObj = serializedObject.FindProperty("disabledIconObj");
            var normalIconObj = serializedObject.FindProperty("normalIconObj");
            var highlightIconObj = serializedObject.FindProperty("highlightIconObj");

            var isInteractable = serializedObject.FindProperty("isInteractable");
            var useLocalization = serializedObject.FindProperty("useLocalization");
            var useUINavigation = serializedObject.FindProperty("useUINavigation");
            var navigationMode = serializedObject.FindProperty("navigationMode");
            var selectOnUp = serializedObject.FindProperty("selectOnUp");
            var selectOnDown = serializedObject.FindProperty("selectOnDown");
            var selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            var selectOnRight = serializedObject.FindProperty("selectOnRight");
            var wrapAround = serializedObject.FindProperty("wrapAround");
            var useSounds = serializedObject.FindProperty("useSounds");
            var fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");

            switch (currentTab)
            {
                case 0:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);

                    if (msTarget.items.Count != 0)
                    {
                        HeatUIEditorHandler.DrawPropertyCW(headerTitle, customSkin, "Header Title", 90);
                        HeatUIEditorHandler.DrawPropertyCW(headerTitleKey, customSkin, "Header Key", "Used for localization.", 90);

                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.BeginHorizontal();

                        GUI.enabled = false;
                        EditorGUILayout.LabelField(new GUIContent("Default Item:"), customSkin.FindStyle("Text"), GUILayout.Width(74));
                        GUI.enabled = true;
                        EditorGUILayout.LabelField(new GUIContent(msTarget.items[currentModeIndex.intValue].title), customSkin.FindStyle("Text"));

                        GUILayout.EndHorizontal();
                        GUILayout.Space(2);

                        currentModeIndex.intValue = EditorGUILayout.IntSlider(currentModeIndex.intValue, 0, msTarget.items.Count - 1);

                        GUILayout.EndVertical();
                    }

                    else { EditorGUILayout.HelpBox("There is no item in the list.", MessageType.Warning); }

                    GUILayout.BeginVertical();
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(items, new GUIContent("Selector Items"), true);

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndVertical();

                    HeatUIEditorHandler.DrawHeader(customSkin, "Events Header", 10);
                    EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
                    break;

                case 1:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    HeatUIEditorHandler.DrawProperty(modeSelectPopup, customSkin, "Mode Select Popup");
                    HeatUIEditorHandler.DrawProperty(transitionPanel, customSkin, "Transition Panel");
                    HeatUIEditorHandler.DrawProperty(itemParent, customSkin, "Item Parent");
                    HeatUIEditorHandler.DrawProperty(itemPreset, customSkin, "Item Preset");
                    HeatUIEditorHandler.DrawProperty(normalCG, customSkin, "Normal CG");
                    HeatUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");
                    HeatUIEditorHandler.DrawProperty(disabledCG, customSkin, "Disabled CG");
                    HeatUIEditorHandler.DrawProperty(disabledHeaderObj, customSkin, "Disabled Header Obj");
                    HeatUIEditorHandler.DrawProperty(normalHeaderObj, customSkin, "Norma lHeader Obj");
                    HeatUIEditorHandler.DrawProperty(highlightHeaderObj, customSkin, "Highlight Header Obj");
                    HeatUIEditorHandler.DrawProperty(disabledTextObj, customSkin, "Disabled Text Obj");
                    HeatUIEditorHandler.DrawProperty(normalTextObj, customSkin, "Normal Text Obj");
                    HeatUIEditorHandler.DrawProperty(highlightTextObj, customSkin, "Highlight Text Obj");
                    HeatUIEditorHandler.DrawProperty(backgroundImage, customSkin, "Background Image");
                    HeatUIEditorHandler.DrawProperty(disabledIconObj, customSkin, "Disabled Icon Obj");
                    HeatUIEditorHandler.DrawProperty(normalIconObj, customSkin, "Normal Icon Obj");
                    HeatUIEditorHandler.DrawProperty(highlightIconObj, customSkin, "Highlight Icon Obj");
                    break;

                case 2:
                    HeatUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    HeatUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");
                    isInteractable.boolValue = HeatUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    useLocalization.boolValue = HeatUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    useSounds.boolValue = HeatUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Button Sounds");
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    useUINavigation.boolValue = HeatUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

                    GUILayout.Space(4);

                    if (useUINavigation.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        HeatUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                        if (msTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                        {
                            EditorGUI.indentLevel = 1;
                            //   GUILayout.Space(-3);
                            wrapAround.boolValue = HeatUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                            //  GUILayout.Space(4);
                            EditorGUI.indentLevel = 0;
                        }

                        else if (msTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                        {
                            wrapAround.boolValue = HeatUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                        }

                        else if (msTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
                        {
                            EditorGUI.indentLevel = 1;
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnUp, customSkin, "Select On Up");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnDown, customSkin, "Select On Down");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnLeft, customSkin, "Select On Left");
                            HeatUIEditorHandler.DrawPropertyPlain(selectOnRight, customSkin, "Select On Right");
                            EditorGUI.indentLevel = 0;
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.EndVertical();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif