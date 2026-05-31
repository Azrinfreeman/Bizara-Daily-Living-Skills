using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AddLangBtnModulUtama : EditorWindow
{
    [MenuItem("Antigravity/Fix Lang Btn Modul Utama")]
    public static void Execute()
    {
        string scenePath = "Assets/Scenes/ModulUtama.unity";
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        
        GameObject parentModeBtn = GameObject.Find("Btn_KunciIbuBapaMod");
        if (parentModeBtn == null) {
            Debug.LogError("Could not find Btn_KunciIbuBapaMod in ModulUtama");
            return;
        }
        
        Transform parent = parentModeBtn.transform.parent;
        
        Transform existing = parent.Find("Btn_LanguageToggle");
        if (existing != null) {
            DestroyImmediate(existing.gameObject);
        }
        
        GameObject langBtn = Instantiate(parentModeBtn, parent);
        langBtn.name = "Btn_LanguageToggle";
        langBtn.transform.SetSiblingIndex(parentModeBtn.transform.GetSiblingIndex()); // Put above Parent Mode
        
        // Ensure LayoutElement exists and has height
        LayoutElement le = langBtn.GetComponent<LayoutElement>();
        if (le == null) le = langBtn.AddComponent<LayoutElement>();
        
        LayoutElement origLe = parentModeBtn.GetComponent<LayoutElement>();
        if (origLe != null) {
            le.minHeight = origLe.minHeight;
            le.preferredHeight = origLe.preferredHeight;
            le.ignoreLayout = origLe.ignoreLayout;
        } else {
            le.minHeight = 60;
            le.preferredHeight = 60;
        }
        
        // Remove click events
        var btn = langBtn.GetComponent<Button>();
        if (btn != null) btn.onClick = new Button.ButtonClickedEvent(); 
        
        // Only destroy specific scripts we know are for ParentMode
        // Usually it's something like ModulUtamaController or similar. 
        // To be safe, we'll only add ButangTukarBahasa and remove TeksTerjemahan
        
        TeksTerjemahan tt = langBtn.GetComponentInChildren<TeksTerjemahan>();
        if (tt != null) DestroyImmediate(tt);
        
        ButangTukarBahasa langScript = langBtn.AddComponent<ButangTukarBahasa>();
        langScript.teksLabel = langBtn.GetComponentInChildren<TextMeshProUGUI>();
        
        if (langScript.teksLabel != null) {
            langScript.teksLabel.text = "English / Melayu"; // Set default text
        }
        
        // Hide icon if present
        Transform icon = langBtn.transform.Find("Icon");
        if (icon != null) icon.gameObject.SetActive(false);
        
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Fixed Language Toggle Button!");
    }
}
