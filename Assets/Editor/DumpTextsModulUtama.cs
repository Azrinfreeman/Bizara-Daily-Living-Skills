using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class DumpTextsModulUtama : EditorWindow
{
    [MenuItem("Antigravity/Dump Texts Modul Utama")]
    public static void Execute()
    {
        string scenePath = "Assets/Scenes/ModulUtama.unity";
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        
        TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        using (StreamWriter writer = new StreamWriter("Assets/ModulUtama_Texts.txt"))
        {
            foreach (var tmp in allTexts)
            {
                writer.WriteLine($"{tmp.gameObject.name} | {tmp.text}");
            }
        }
        
        Debug.Log("Dumped " + allTexts.Length + " texts to Assets/ModulUtama_Texts.txt");
    }
}
