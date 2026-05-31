using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class DumpAllModulTexts : EditorWindow
{
    [MenuItem("Antigravity/Dump All Modul Texts")]
    public static void Execute()
    {
        string[] scenes = new string[] { 
            "Assets/Scenes/Modul1.unity", 
            "Assets/Scenes/Modul2.unity", 
            "Assets/Scenes/Modul3.unity", 
            "Assets/Scenes/Modul4.unity" 
        };
        
        HashSet<string> uniqueTexts = new HashSet<string>();

        foreach (string path in scenes)
        {
            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach (var tmp in allTexts)
            {
                string rawText = tmp.text.Trim();
                if (!string.IsNullOrEmpty(rawText) && rawText.Length > 1)
                {
                    uniqueTexts.Add(rawText);
                }
            }
        }
        
        using (StreamWriter writer = new StreamWriter("Assets/All_Modul_Texts.txt"))
        {
            foreach (var t in uniqueTexts)
            {
                writer.WriteLine(t.Replace("\n", "\\n"));
            }
        }
        
        Debug.Log("Dumped " + uniqueTexts.Count + " unique texts to Assets/All_Modul_Texts.txt");
    }
}
