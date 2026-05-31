using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ApplySidebarToAllModuls : EditorWindow
{
    [MenuItem("Antigravity/Apply Sidebar to All Moduls")]
    public static void ApplySidebar()
    {
        string[] scenes = new string[]
        {
            "Assets/Scenes/Modul1.unity",
            "Assets/Scenes/Modul2.unity",
            "Assets/Scenes/Modul3.unity",
            "Assets/Scenes/Modul4.unity"
        };

        foreach (string scenePath in scenes)
        {
            // Open scene
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogWarning($"Canvas not found in {scenePath}");
                continue;
            }

            Transform bg = canvas.transform.Find("background");
            if (bg == null)
            {
                Debug.LogWarning($"Canvas/background not found in {scenePath}");
                continue;
            }

            // 1. Setup Background Color
            Image bgImage = bg.GetComponent<Image>();
            if (bgImage != null)
            {
                ColorUtility.TryParseHtmlString("#F2F4F7", out Color bgColor);
                bgImage.color = bgColor;
            }

            // 2. Adjust existing content to make room for Sidebar (Shift right to 0.25)
            foreach (Transform child in bg)
            {
                if (child.name == "Sidebar" || child.name == "Title" || child.name == "ModButton") continue;

                RectTransform rect = child.GetComponent<RectTransform>();
                if (rect != null)
                {
                    // Adjust anchor Min X to 0.25 to leave room for sidebar
                    Vector2 min = rect.anchorMin;
                    min.x = Mathf.Max(min.x, 0.25f);
                    rect.anchorMin = min;

                    // Ensure offset is zero so it fills the anchored area
                    rect.offsetMin = new Vector2(0, rect.offsetMin.y);
                }
            }

            // 3. Create Sidebar
            Transform existingSidebar = bg.Find("Sidebar");
            if (existingSidebar != null) DestroyImmediate(existingSidebar.gameObject);

            GameObject sidebarObj = new GameObject("Sidebar");
            sidebarObj.transform.SetParent(bg, false);
            sidebarObj.transform.SetSiblingIndex(0); // Put it at the top of hierarchy to render behind other stuff if needed, or actually it doesn't matter much.
            RectTransform sidebarRect = sidebarObj.AddComponent<RectTransform>();
            sidebarRect.anchorMin = new Vector2(0, 0);
            sidebarRect.anchorMax = new Vector2(0.22f, 1);
            sidebarRect.sizeDelta = Vector2.zero;
            sidebarRect.anchoredPosition = Vector2.zero;

            Image sidebarImage = sidebarObj.AddComponent<Image>();
            ColorUtility.TryParseHtmlString("#1965B2", out Color sidebarColor);
            sidebarImage.color = sidebarColor;

            // Logo Area
            GameObject logoArea = new GameObject("LogoArea");
            logoArea.transform.SetParent(sidebarObj.transform, false);
            RectTransform logoRect = logoArea.AddComponent<RectTransform>();
            logoRect.anchorMin = new Vector2(0, 0.8f);
            logoRect.anchorMax = new Vector2(1, 1);
            logoRect.sizeDelta = Vector2.zero;

            CreateText(logoArea.transform, "BizaraText", "BIZARA", 32, true, Color.white, new Vector2(0, 0.3f), new Vector2(1, 0.8f));
            CreateText(logoArea.transform, "SayaBolehText", "Saya Boleh!", 24, false, Color.white, new Vector2(0, 0), new Vector2(1, 0.3f));

            // Menu Area
            GameObject menuArea = new GameObject("MenuArea");
            menuArea.transform.SetParent(sidebarObj.transform, false);
            RectTransform menuRect = menuArea.AddComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0, 0);
            menuRect.anchorMax = new Vector2(1, 0.8f);
            menuRect.sizeDelta = Vector2.zero;

            VerticalLayoutGroup menuLayout = menuArea.AddComponent<VerticalLayoutGroup>();
            menuLayout.padding = new RectOffset(20, 20, 20, 20);
            menuLayout.spacing = 15;
            menuLayout.childControlHeight = true;
            menuLayout.childControlWidth = true;
            menuLayout.childForceExpandHeight = false;

            string sceneName = scene.name;
            CreateMenuButton(menuArea.transform, "Home", false);
            CreateMenuButton(menuArea.transform, "Eating & Drinking", sceneName == "Modul1");
            CreateMenuButton(menuArea.transform, "Toilet Time", sceneName == "Modul2");
            CreateMenuButton(menuArea.transform, "I Am Clean", sceneName == "Modul3");
            CreateMenuButton(menuArea.transform, "I Am Organized", sceneName == "Modul4");

            // Save Scene
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Applied sidebar to {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished applying sidebar to all Modul scenes.");
    }

    private static void CreateText(Transform parent, string name, string text, float size, bool isBold, Color color, Vector2 min, Vector2 max)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = isBold ? FontStyles.Bold : FontStyles.Normal;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    private static void CreateMenuButton(Transform parent, string text, bool isActive)
    {
        GameObject obj = new GameObject("Btn_" + text);
        obj.transform.SetParent(parent, false);
        
        LayoutElement le = obj.AddComponent<LayoutElement>();
        le.minHeight = 60;

        Image img = obj.AddComponent<Image>();
        ColorUtility.TryParseHtmlString(isActive ? "#F0F4F8" : "#1A5798", out Color bgColor);
        img.color = bgColor;

        CreateText(obj.transform, "Text", text, 20, isActive, isActive ? new Color(0.1f, 0.4f, 0.7f) : Color.white, new Vector2(0, 0), new Vector2(1, 1));
    }
}
