using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Beautify.Universal {

    public class ContactUsWindow : EditorWindow {
        const string OnlineGuidesUrl = "https://kronnect.com/guides";
        const string SupportUrl = "https://kronnect.com/support";
        const string YoutubeUrl = "https://youtube.com/@kronnect";
        const string TwitterUrl = "https://twitter.com/kronnect";
        const string KronnectUrl = "https://assetstore.unity.com/publishers/15018?aid=1101lGsd";
        const string ReferralInfo = "?aid=1101lGsd&pubref=beautify";
        Texture2D kronnectLogo, welcomeBanner;
        GUIStyle headerStyle, bannerStyle;

        readonly List<AssetCategory> assetCategories = new List<AssetCategory>
    {
    new AssetCategory
    {
        name = "Image Effects",
        assets = new List<Asset>
        {
            new Asset { name = "Beautify 3 - Advanced Post Processing", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/beautify-3-advanced-post-processing-233073" },
            new Asset { name = "Frame Pack for Beautify", url = "https://assetstore.unity.com/packages/2d/gui/frame-pack-204058" },
            new Asset { name = "LUT Pack for Beautify", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/lut-pack-for-beautify-202502" },
            new Asset { name = "Beautify HDRP", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/beautify-hdrp-165411" },
            new Asset { name = "Cloud Shadows FX", url = "https://assetstore.unity.com/packages/vfx/shaders/cloud-shadows-fx-267702" },
            new Asset { name = "Dynamic Fog & Mist 2", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/dynamic-fog-mist-2-48200" },
            new Asset { name = "Global Snow 2", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/global-snow-2-248191" },
            new Asset { name = "Luma Based Ambient Occlusion (SSAO 2D)", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/luma-based-ambient-occlusion-2-ssao-2d-249066" },
            new Asset { name = "Radiant Global Illumination", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/radiant-global-illumination-225934" },
            new Asset { name = "Shiny SSR 2", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/shiny-ssr-2-screen-space-reflections-188638" },
            new Asset { name = "Sun Flares HDRP", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/sun-flares-hdrp-171177" },
            new Asset { name = "Umbra Soft Shadows", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/umbra-soft-shadows-better-directional-contact-shadows-for-urp-282485" },
            new Asset { name = "Volumetric Fog & Mist 2", url = "https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/volumetric-fog-mist-2-162694" },
            new Asset { name = "Volumetric Lights 2", url = "https://assetstore.unity.com/packages/vfx/shaders/volumetric-lights-2-234539" },
            new Asset { name = "Volumetric Lights Set", url = "https://assetstore.unity.com/packages/3d/props/volumetric-lights-set-237873" },
            new Asset { name = "Volumetric Lights 2 HDRP", url = "https://assetstore.unity.com/packages/vfx/shaders/volumetric-lights-2-hdrp-243807" },
        }
    },
    new AssetCategory
    {
        name = "Tools & Shaders",
        assets = new List<Asset>
        {
            new Asset { name = "Compass Navigator Pro 2", url = "https://assetstore.unity.com/packages/tools/gui/compass-navigator-pro-2-273662" },
            new Asset { name = "Highlight Plus", url = "https://assetstore.unity.com/packages/vfx/shaders/highlight-plus-all-in-one-outline-selection-effects-134149" },
            new Asset { name = "Highlight Plus 2D", url = "https://assetstore.unity.com/packages/vfx/shaders/highlight-plus-2d-138383" },
            new Asset { name = "Liquid Volume 2", url = "https://assetstore.unity.com/packages/vfx/shaders/liquid-volume-2-249127" },
            new Asset { name = "Liquid Volume Pro 2", url = "https://assetstore.unity.com/packages/vfx/shaders/liquid-volume-pro-2-129967" },
            new Asset { name = "Liquid Volume Pro 2 HDRP", url = "https://assetstore.unity.com/packages/vfx/shaders/liquid-volume-pro-2-hdrp-253786" },
            new Asset { name = "Potions & Volumetric Liquid", url = "https://assetstore.unity.com/packages/slug/123474" },
            new Asset { name = "Shader Control", url = "https://assetstore.unity.com/packages/vfx/shaders/shader-control-74817" },
            new Asset { name = "Split Screen Pro", url = "https://assetstore.unity.com/packages/tools/camera/split-screen-pro-207149" },
            new Asset { name = "Skybox Plus", url = "https://assetstore.unity.com/packages/2d/environments/skybox-plus-182966" },
            new Asset { name = "Trails FX", url = "https://assetstore.unity.com/packages/vfx/shaders/trails-fx-146898" },
            new Asset { name = "Transitions Plus", url = "https://assetstore.unity.com/packages/tools/camera/transitions-plus-266067" },
            new Asset { name = "Tunnel FX 2", url = "https://assetstore.unity.com/packages/vfx/shaders/tunnel-fx-2-86544" },
            new Asset { name = "Voxel Play 2", url = "https://assetstore.unity.com/packages/tools/game-toolkits/voxel-play-2-201234" },
            new Asset { name = "Pirates of Voxel Play", url = "https://assetstore.unity.com/packages/tools/game-toolkits/pirates-of-voxel-play-189096" },
            new Asset { name = "X-Frame FPS Accelerator", url = "https://assetstore.unity.com/packages/tools/camera/x-frame-fps-accelerator-63965" }
        }
    },
    new AssetCategory
    {
        name = "Grids & Maps",
        assets = new List<Asset>
        {
            new Asset { name = "Grids 2D", url = "https://assetstore.unity.com/packages/tools/game-toolkits/grids-2d-59981" },
            new Asset { name = "Hexasphere Grid System", url = "https://assetstore.unity.com/packages/tools/modeling/hexasphere-grid-system-89112" },
            new Asset { name = "Terrain Grid System 2", url = "https://assetstore.unity.com/packages/tools/terrain/terrain-grid-system-2-244921" },
            new Asset { name = "World Map 2D Edition 2", url = "https://assetstore.unity.com/packages/tools/gui/world-map-2d-edition-2-151238" },
            new Asset { name = "World Map Globe Edition 2", url = "https://assetstore.unity.com/packages/tools/gui/world-map-globe-edition-2-150643" },
            new Asset { name = "World Map Strategy Kit 2", url = "https://assetstore.unity.com/packages/tools/game-toolkits/world-map-strategy-kit-2-150938" },
            new Asset { name = "World Maps & Weather Symbols", url = "https://assetstore.unity.com/packages/2d/textures-materials/world-flags-and-weather-symbols-69010" },
            new Asset { name = "Military Units 2D", url = "https://assetstore.unity.com/packages/2d/textures-materials/military-units-the-stylized-art-collection-187769" },
            new Asset { name = "Military Units 3D", url = "https://assetstore.unity.com/packages/3d/vehicles/military-units-3d-246876" },
        }
    }
};


        void OnEnable () {
            kronnectLogo = Resources.Load<Texture2D>("kronnectLogo");
            welcomeBanner = Resources.Load<Texture2D>("welcomeBanner");
        }

        void DrawHeader (string title) {
            if (headerStyle == null) {
                GUIStyle skurikenModuleTitleStyle = "ShurikenModuleTitle";
                headerStyle = new GUIStyle(skurikenModuleTitleStyle) {
                    contentOffset = new Vector2(5f, -2f),
                    normal = { textColor = Color.white },
                    fixedHeight = 24,
                    fontSize = 13
                };
            }

            GUILayout.Label(title, headerStyle);
        }

        void OnGUI () {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);

            DrawHeader("Online Resources");

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            if (GUILayout.Button("Asset Documentation", EditorStyles.linkLabel)) {
                Application.OpenURL(OnlineGuidesUrl);
            }
            if (GUILayout.Button("Support & Community", EditorStyles.linkLabel)) {
                Application.OpenURL(SupportUrl);
            }
            if (GUILayout.Button("YouTube Channel", EditorStyles.linkLabel)) {
                Application.OpenURL(YoutubeUrl);
            }
            if (GUILayout.Button("X / Twitter", EditorStyles.linkLabel)) {
                Application.OpenURL(TwitterUrl);
            }
            if (GUILayout.Button("Kronnect Asset Store", EditorStyles.linkLabel)) {
                Application.OpenURL(KronnectUrl);
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();

            if (bannerStyle == null) {
                bannerStyle = new GUIStyle(GUI.skin.button);
                bannerStyle.normal.background = welcomeBanner;
                bannerStyle.normal.scaledBackgrounds = new[] { welcomeBanner };
            }
            const float width = 1100 / 2;
            const float height = 200 / 2;

            if (GUILayout.Button("", bannerStyle, GUILayout.Width(width), GUILayout.Height(height))) {
                Application.OpenURL(KronnectUrl);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            DrawHeader("Kronnect Assets");

            GUIStyle textStyle = new GUIStyle(EditorStyles.wordWrappedLabel) {
                fontSize = 11
            };
            GUILayout.Label("Thank you for using this asset!\nWe invite you to explore more assets and complete your collection by visiting the affiliated links below to the Asset Store:", textStyle);

            EditorGUILayout.Space(10);
            GUILayout.BeginHorizontal();

            foreach (var category in assetCategories) {
                GUILayout.BeginVertical();
                GUILayout.Label(category.name, EditorStyles.boldLabel);
                foreach (var asset in category.assets) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    if (GUILayout.Button(asset.name, EditorStyles.linkLabel)) {
                        Application.OpenURL(asset.url + ReferralInfo);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Close", GUILayout.Width(60))) {
                Close();
            }
            EditorGUILayout.Space();
            GUILayout.Label(new GUIContent(kronnectLogo), GUILayout.Width(100), GUILayout.Height(30));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }

        public static void ShowScreen () {
            ContactUsWindow window = GetWindow<ContactUsWindow>(true, "Online Resources", true);
            window.minSize = window.maxSize = new Vector2(750, 640);
        }

        [System.Serializable]
        public class AssetCategory {
            public string name;
            public List<Asset> assets;
        }

        [System.Serializable]
        public class Asset {
            public string name;
            public string url;
        }
    }

}