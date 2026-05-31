using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;

public class FixSidebarButtons : EditorWindow
{
    [MenuItem("Antigravity/Fix Sidebar Prefab Buttons")]
    public static void FixButtons()
    {
        string prefabPath = "Assets/Prefabs/Sidebar.prefab";
        
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        if (prefabRoot == null)
        {
            Debug.LogError("Failed to load Sidebar prefab.");
            return;
        }

        SidebarController controller = prefabRoot.GetComponent<SidebarController>();
        if (controller == null)
        {
            Debug.LogError("SidebarController not found on prefab.");
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return;
        }

        Transform menuArea = prefabRoot.transform.Find("MenuArea");
        if (menuArea != null)
        {
            LinkButton(menuArea, "Btn_Home", controller, "ModulUtama");
            LinkButton(menuArea, "Btn_Eating & Drinking", controller, "Modul1");
            LinkButton(menuArea, "Btn_Toilet Time", controller, "Modul2");
            LinkButton(menuArea, "Btn_I Am Clean", controller, "Modul3");
            LinkButton(menuArea, "Btn_I Am Organized", controller, "Modul4");
        }

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
        Debug.Log("Successfully wired up Sidebar prefab buttons!");
    }

    private static void LinkButton(Transform menuArea, string btnName, SidebarController controller, string targetScene)
    {
        Transform btnTransform = menuArea.Find(btnName);
        if (btnTransform == null) return;

        Button button = btnTransform.GetComponent<Button>();
        if (button == null) return;

        // Clear existing persistent listeners (which broke across scenes)
        while (button.onClick.GetPersistentEventCount() > 0)
        {
            UnityEventTools.RemovePersistentListener(button.onClick, 0);
        }

        // Add the persistent listener pointing to the Prefab's own SidebarController
        var methodInfo = typeof(SidebarController).GetMethod("GoToScene");
        if (methodInfo != null)
        {
            var action = System.Delegate.CreateDelegate(typeof(UnityAction<string>), controller, methodInfo) as UnityAction<string>;
            UnityEventTools.AddStringPersistentListener(button.onClick, action, targetScene);
        }
    }
}
