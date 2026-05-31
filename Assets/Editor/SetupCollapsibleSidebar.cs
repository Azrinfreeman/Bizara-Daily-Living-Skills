using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;
using System.Collections.Generic;

public class SetupCollapsibleSidebar : EditorWindow
{
    [MenuItem("Antigravity/Setup Collapsible Sidebar")]
    public static void Setup()
    {
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

            Transform bg = canvas.transform.Find("background");
            if (bg == null) continue;

            Transform sidebar = bg.Find("Sidebar");
            if (sidebar == null) continue;

            // 1. Add/Get SidebarController
            SidebarController controller = sidebar.GetComponent<SidebarController>();
            if (controller == null)
            {
                controller = sidebar.gameObject.AddComponent<SidebarController>();
            }

            controller.sidebarRect = sidebar.GetComponent<RectTransform>();
            controller.contentElements = new List<RectTransform>();
            controller.sidebarOpenAnchorX = 0.22f;

            // Gather content elements to push
            foreach (Transform child in bg)
            {
                if (child == sidebar || child.name == "ToggleSidebarButton") continue;
                
                RectTransform childRect = child.GetComponent<RectTransform>();
                if (childRect != null && childRect.anchorMin.x > 0.1f)
                {
                    controller.contentElements.Add(childRect);
                }
            }

            // 2. Create Hamburger Toggle Button
            Transform toggleBtnTransform = bg.Find("ToggleSidebarButton");
            if (toggleBtnTransform != null) DestroyImmediate(toggleBtnTransform.gameObject);

            GameObject toggleBtnObj = new GameObject("ToggleSidebarButton");
            toggleBtnObj.transform.SetParent(bg, false);
            toggleBtnObj.transform.SetAsLastSibling(); // render on top of everything

            RectTransform toggleRect = toggleBtnObj.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0, 1);
            toggleRect.anchorMax = new Vector2(0, 1);
            toggleRect.sizeDelta = new Vector2(50, 50);
            toggleRect.anchoredPosition = new Vector2(35, -35); // 10px padding from top left + 25px half size

            Image toggleImg = toggleBtnObj.AddComponent<Image>();
            toggleImg.color = Color.white; // Or any style

            // Hamburger Text Icon
            GameObject textObj = new GameObject("Icon");
            textObj.transform.SetParent(toggleBtnObj.transform, false);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TMPro.TextMeshProUGUI tmp = textObj.AddComponent<TMPro.TextMeshProUGUI>();
            tmp.text = "≡";
            tmp.fontSize = 40;
            tmp.alignment = TMPro.TextAlignmentOptions.Center;
            ColorUtility.TryParseHtmlString("#1965B2", out Color blueColor);
            tmp.color = blueColor;

            Button btn = toggleBtnObj.AddComponent<Button>();
            
            // Clear existing and link
            while (btn.onClick.GetPersistentEventCount() > 0)
            {
                UnityEventTools.RemovePersistentListener(btn.onClick, 0);
            }

            var methodInfo = typeof(SidebarController).GetMethod("ToggleSidebar");
            if (methodInfo != null)
            {
                var action = System.Delegate.CreateDelegate(typeof(UnityAction), controller, methodInfo) as UnityAction;
                UnityEventTools.AddPersistentListener(btn.onClick, action);
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Setup collapsible sidebar in {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished setting up Collapsible Sidebar in all scenes.");
    }
}
