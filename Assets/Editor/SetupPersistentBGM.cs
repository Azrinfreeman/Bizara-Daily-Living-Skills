using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupPersistentBGM : EditorWindow
{
    [MenuItem("Antigravity/Setup Persistent BGM")]
    public static void Execute()
    {
        // Find the GameObject in the active scene
        GameObject bgmObject = GameObject.Find("Marble_and_Morning");
        
        if (bgmObject == null)
        {
            // Also try to find it without exact name match if there's a space or something
            bgmObject = GameObject.Find("Marble and Morning");
        }

        if (bgmObject != null)
        {
            // Attach PersistentBGM script if not already attached
            if (bgmObject.GetComponent<PersistentBGM>() == null)
            {
                bgmObject.AddComponent<PersistentBGM>();
                EditorSceneManager.MarkSceneDirty(bgmObject.scene);
                Debug.Log("Successfully attached PersistentBGM to " + bgmObject.name);
            }
            else
            {
                Debug.Log("PersistentBGM is already attached to " + bgmObject.name);
            }
            
            // Also check if they want to update the targetBGM in the VolumeController
            // Wait, we can't easily find it globally but if BGMVolumeController is in the scene we could link it.
            // (Our BGMVolumeController script falls back to AudioListener.volume anyway).
        }
        else
        {
            Debug.LogError("Could not find a GameObject named 'Marble_and_Morning' in the active scene.");
        }
    }
}
