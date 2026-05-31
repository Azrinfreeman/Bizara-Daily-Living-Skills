using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;
using System.Collections.Generic;

public class MakeSidebarPrefab : EditorWindow
{
    [MenuItem("Antigravity/Make Sidebar Prefab")]
    public static void MakePrefab()
    {
        string prefabsFolder = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabsFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string prefabPath = "Assets/Prefabs/Sidebar.prefab";

        // Step 1: Create the Prefab from ModulUtama
        var mainScene = EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        GameObject canvas = GameObject.Find("Canvas");
        Transform bg = canvas != null ? canvas.transform.Find("background") : null;
        Transform sidebar = bg != null ? bg.Find("Sidebar") : null;

        if (sidebar == null)
        {
            Debug.LogError("Could not find Sidebar in ModulUtama to create prefab.");
            return;
        }

        // Add ActiveState script to handle visual states dynamically
        if (sidebar.GetComponent<SidebarActiveState>() == null)
        {
            sidebar.gameObject.AddComponent<SidebarActiveState>();
        }

        // Save as Prefab
        GameObject prefabObj = PrefabUtility.SaveAsPrefabAssetAndConnect(sidebar.gameObject, prefabPath, InteractionMode.AutomatedAction);
        if (prefabObj == null)
        {
            Debug.LogError("Failed to save Sidebar prefab.");
            return;
        }

        EditorSceneManager.MarkSceneDirty(mainScene);
        EditorSceneManager.SaveScene(mainScene);
        Debug.Log("Created Sidebar.prefab and connected in ModulUtama.");

        // Step 2: Replace in other scenes
        string[] otherScenes = new string[]
        {
            "Assets/Scenes/Modul1.unity",
            "Assets/Scenes/Modul2.unity",
            "Assets/Scenes/Modul3.unity",
            "Assets/Scenes/Modul4.unity"
        };

        foreach (string scenePath in otherScenes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            canvas = GameObject.Find("Canvas");
            bg = canvas != null ? canvas.transform.Find("background") : null;
            if (bg == null) continue;

            Transform oldSidebar = bg.Find("Sidebar");
            if (oldSidebar != null)
            {
                DestroyImmediate(oldSidebar.gameObject);
            }

            // Instantiate Prefab
            GameObject newSidebar = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj);
            newSidebar.name = "Sidebar";
            newSidebar.transform.SetParent(bg, false);
            newSidebar.transform.SetSiblingIndex(0);

            // Re-setup SidebarController specific to this scene
            SidebarController controller = newSidebar.GetComponent<SidebarController>();
            if (controller != null)
            {
                controller.contentElements = new List<RectTransform>();
                foreach (Transform child in bg)
                {
                    if (child == newSidebar.transform || child.name == "ToggleSidebarButton" || child.name == "Title" || child.name == "ModButton") continue;
                    
                    RectTransform childRect = child.GetComponent<RectTransform>();
                    if (childRect != null && childRect.anchorMin.x > 0.1f)
                    {
                        controller.contentElements.Add(childRect);
                    }
                }
                
                // Record the override for the instance
                PrefabUtility.RecordPrefabInstancePropertyModifications(controller);
            }

            // Re-link Toggle Button
            Transform toggleBtnTransform = bg.Find("ToggleSidebarButton");
            if (toggleBtnTransform != null)
            {
                Button btn = toggleBtnTransform.GetComponent<Button>();
                if (btn != null)
                {
                    while (btn.onClick.GetPersistentEventCount() > 0)
                    {
                        UnityEventTools.RemovePersistentListener(btn.onClick, 0);
                    }

                    var methodInfo = typeof(SidebarController).GetMethod("ToggleSidebar");
                    if (methodInfo != null && controller != null)
                    {
                        var action = System.Delegate.CreateDelegate(typeof(UnityAction), controller, methodInfo) as UnityAction;
                        UnityEventTools.AddPersistentListener(btn.onClick, action);
                    }
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Replaced sidebar with prefab in {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished making Sidebar a Prefab across all scenes.");
    }
}
