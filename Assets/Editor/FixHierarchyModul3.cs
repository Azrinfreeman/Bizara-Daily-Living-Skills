using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixHierarchyModul3
{
    [MenuItem("Tools/Fix Modul3 Hierarchy")]
    public static void Fix()
    {
        var root = GameObject.Find("SlotAnswer");
        if (root != null)
        {
            var box = root.transform.Find("Box1 (4)");
            var image = root.transform.Find("Image (2)");
            if (box != null && image != null)
            {
                box.SetSiblingIndex(4);
                image.SetAsLastSibling();
                EditorUtility.SetDirty(root);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(root.gameObject.scene);
                Debug.Log("Fixed sibling index for Modul3");
            }
            else
            {
                Debug.LogWarning("Could not find Box1 (4) or Image (2)");
            }
        }
        else
        {
            Debug.LogWarning("Could not find SlotAnswer");
        }
    }
}
