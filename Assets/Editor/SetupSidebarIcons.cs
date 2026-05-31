using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetupSidebarIcons : EditorWindow
{
    [MenuItem("Antigravity/Setup Sidebar Icons")]
    public static void Setup()
    {
        Dictionary<string, string> buttonToIconPath = new Dictionary<string, string>
        {
            { "Btn_Home", "Assets/Sprites/Icons/icon_home.png" },
            { "Btn_Eating & Drinking", "Assets/Sprites/Icons/icon_eating.png" },
            { "Btn_Toilet Time", "Assets/Sprites/Icons/icon_toilet.png" },
            { "Btn_I Am Clean", "Assets/Sprites/Icons/icon_clean.png" },
            { "Btn_I Am Organized", "Assets/Sprites/Icons/icon_organized.png" }
        };

        // 1. Ensure assets are imported as Sprites
        foreach (var path in buttonToIconPath.Values)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
        }

        string[] scenes = new string[]
        {
            "Assets/Scenes/ModulUtama.unity",
            "Assets/Scenes/Modul1.unity",
            "Assets/Scenes/Modul2.unity",
            "Assets/Scenes/Modul3.unity",
            "Assets/Scenes/Modul4.unity"
        };

        foreach (string scenePath in scenes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null) continue;

            Transform menuArea = canvas.transform.Find("background/Sidebar/MenuArea");
            if (menuArea == null) continue;

            foreach (var kvp in buttonToIconPath)
            {
                string btnName = kvp.Key;
                string iconPath = kvp.Value;
                
                Transform btnTransform = menuArea.Find(btnName);
                if (btnTransform == null) continue;

                // Make inactive buttons visible rounded rects (adjusting color)
                Image bgImage = btnTransform.GetComponent<Image>();
                ColorUtility.TryParseHtmlString("#F0F4F8", out Color activeColor);
                
                if (bgImage != null && bgImage.color != activeColor)
                {
                    // It's inactive. Let's make it a slightly distinct blue
                    ColorUtility.TryParseHtmlString("#1C60A6", out Color inactiveColor);
                    bgImage.color = inactiveColor;
                }

                // Move text to the right
                Transform textTransform = btnTransform.Find("Text");
                if (textTransform != null)
                {
                    RectTransform textRect = textTransform.GetComponent<RectTransform>();
                    textRect.anchorMin = new Vector2(0.3f, 0);
                    textRect.anchorMax = new Vector2(1, 1);
                    textRect.sizeDelta = Vector2.zero;
                    
                    TMPro.TextMeshProUGUI tmp = textTransform.GetComponent<TMPro.TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        tmp.alignment = TMPro.TextAlignmentOptions.Left | TMPro.TextAlignmentOptions.Capline;
                    }
                }

                // Add or update Icon Image
                Transform iconTransform = btnTransform.Find("IconImage");
                GameObject iconObj;
                Image iconImage;

                if (iconTransform == null)
                {
                    iconObj = new GameObject("IconImage");
                    iconObj.transform.SetParent(btnTransform, false);
                    iconImage = iconObj.AddComponent<Image>();
                }
                else
                {
                    iconObj = iconTransform.gameObject;
                    iconImage = iconObj.GetComponent<Image>();
                }

                Sprite iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
                if (iconSprite != null)
                {
                    iconImage.sprite = iconSprite;
                    iconImage.preserveAspect = true;
                }

                RectTransform iconRect = iconObj.GetComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0.05f, 0.2f);
                iconRect.anchorMax = new Vector2(0.25f, 0.8f);
                iconRect.sizeDelta = Vector2.zero;
                iconRect.anchoredPosition = Vector2.zero;
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Setup sidebar icons in {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished setting up Sidebar Icons in all scenes.");
    }
}
