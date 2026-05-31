using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;

public class LinkSidebarButtons : EditorWindow
{
    [MenuItem("Antigravity/Link Sidebar Buttons")]
    public static void LinkButtons()
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
            if (canvas == null)
            {
                Debug.LogWarning($"Canvas not found in {scenePath}");
                continue;
            }

            // Ensure ModuleLoader exists in the scene
            ModuleLoader loader = Object.FindFirstObjectByType<ModuleLoader>();
            if (loader == null)
            {
                GameObject loaderObj = new GameObject("SceneLoader");
                loader = loaderObj.AddComponent<ModuleLoader>();
            }

            Transform menuArea = canvas.transform.Find("background/Sidebar/MenuArea");
            if (menuArea == null)
            {
                Debug.LogWarning($"Sidebar/MenuArea not found in {scenePath}");
                continue;
            }

            LinkButton(menuArea, "Btn_Home", loader, "ModulUtama");
            LinkButton(menuArea, "Btn_Eating & Drinking", loader, "Modul1");
            LinkButton(menuArea, "Btn_Toilet Time", loader, "Modul2");
            LinkButton(menuArea, "Btn_I Am Clean", loader, "Modul3");
            LinkButton(menuArea, "Btn_I Am Organized", loader, "Modul4");

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Linked buttons in {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished linking sidebar buttons in all scenes.");
    }

    private static void LinkButton(Transform menuArea, string btnName, ModuleLoader loader, string targetScene)
    {
        Transform btnTransform = menuArea.Find(btnName);
        if (btnTransform == null) return;

        Button button = btnTransform.GetComponent<Button>();
        if (button == null)
        {
            button = btnTransform.gameObject.AddComponent<Button>();
        }

        // Clear existing persistent listeners
        while (button.onClick.GetPersistentEventCount() > 0)
        {
            UnityEventTools.RemovePersistentListener(button.onClick, 0);
        }

        // Add the persistent listener to call loader.LoadModuleScene(targetScene)
        var methodInfo = typeof(ModuleLoader).GetMethod("LoadModuleScene");
        if (methodInfo != null)
        {
            var action = System.Delegate.CreateDelegate(typeof(UnityAction<string>), loader, methodInfo) as UnityAction<string>;
            UnityEventTools.AddStringPersistentListener(button.onClick, action, targetScene);
        }
    }
}
