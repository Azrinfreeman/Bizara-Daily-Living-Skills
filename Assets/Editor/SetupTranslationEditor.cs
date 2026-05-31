using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class SetupTranslationEditor : EditorWindow
{
    [MenuItem("Antigravity/1. Setup Translation Data")]
    public static void ExecuteData()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        DataTerjemahan data = AssetDatabase.LoadAssetAtPath<DataTerjemahan>("Assets/Resources/DataTerjemahan.asset");
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<DataTerjemahan>();
            AssetDatabase.CreateAsset(data, "Assets/Resources/DataTerjemahan.asset");
        }
        
        AddKey(data, "saya_boleh", "Saya Boleh!", "I Can Do It!");
        AddKey(data, "btn_home", "Utama", "Home");
        AddKey(data, "btn_eating", "Makan & Minum", "Eating & Drinking");
        AddKey(data, "btn_toilet", "Masa Tandas", "Toilet Time");
        AddKey(data, "btn_clean", "Saya Bersih", "I Am Clean");
        AddKey(data, "btn_organized", "Saya Kemas", "I Am Organized");
        AddKey(data, "btn_parent_mode", "Mod Ibu Bapa", "Parent Mode");
        
        AddKey(data, "dash_title", "Papan Pemuka Ibu Bapa & Guru", "Parent & Teacher Dashboard");
        AddKey(data, "btn_close", "Tutup", "Close");
        AddKey(data, "tab_profile", "Profil Anak", "Child Profile");
        AddKey(data, "tab_media", "Urus Imej & Video", "Manage Images & Video");
        AddKey(data, "tab_progress", "Kemajuan & CSV", "Progress & CSV");
        AddKey(data, "tab_settings", "Tetapan Sistem", "System Settings");
        
        AddKey(data, "lbl_child_name", "Nama Anak", "Child Name");
        AddKey(data, "lbl_age", "Umur (Tahun)", "Age (Years)");
        AddKey(data, "lbl_select_avatar", "Pilih Avatar", "Select Avatar");
        AddKey(data, "btn_save_profile", "Simpan Profil", "Save Profile");
        
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        
        Debug.Log("1. Translation Data Setup Complete!");
    }

    private static void AddKey(DataTerjemahan data, string key, string my, string en)
    {
        if (!data.senaraiTerjemahan.Exists(x => x.kunci == key))
        {
            data.senaraiTerjemahan.Add(new KunciTerjemahan { kunci = key, teksMelayu = my, teksInggeris = en });
        }
    }

    [MenuItem("Antigravity/2. Setup Sidebar Translation")]
    public static void SetupSidebar()
    {
        string prefabPath = "Assets/Prefabs/Sidebar.prefab";
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        Transform menuArea = prefabRoot.transform.Find("MenuArea");
        
        SetTranslationKey(menuArea.Find("Title/Text"), "saya_boleh");
        SetTranslationKey(menuArea.Find("Btn_Home/Text"), "btn_home");
        SetTranslationKey(menuArea.Find("Btn_Eating/Text"), "btn_eating");
        SetTranslationKey(menuArea.Find("Btn_Toilet/Text"), "btn_toilet");
        SetTranslationKey(menuArea.Find("Btn_Clean/Text"), "btn_clean");
        SetTranslationKey(menuArea.Find("Btn_Organized/Text"), "btn_organized");
        SetTranslationKey(menuArea.Find("Btn_ParentMode/Text"), "btn_parent_mode");

        Transform parentModeBtn = menuArea.Find("Btn_ParentMode");
        Transform existingLangBtn = menuArea.Find("Btn_LanguageToggle");
        
        if (existingLangBtn == null && parentModeBtn != null)
        {
            GameObject langObj = Instantiate(parentModeBtn.gameObject, menuArea);
            langObj.name = "Btn_LanguageToggle";
            langObj.transform.SetSiblingIndex(parentModeBtn.GetSiblingIndex()); // Put above ParentMode
            
            var nav = langObj.GetComponent<Button>();
            nav.onClick = new Button.ButtonClickedEvent(); 
            
            TeksTerjemahan oldTrans = langObj.GetComponentInChildren<TeksTerjemahan>();
            if (oldTrans != null) DestroyImmediate(oldTrans);
            
            ButangTukarBahasa langScript = langObj.AddComponent<ButangTukarBahasa>();
            langScript.teksLabel = langObj.GetComponentInChildren<TextMeshProUGUI>();
            
            Transform icon = langObj.transform.Find("Icon");
            if (icon != null) icon.gameObject.SetActive(false);
            
            Debug.Log("Language Toggle button added!");
        }

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);
        Debug.Log("2. Sidebar Translation Setup Complete!");
    }

    [MenuItem("Antigravity/3. Setup ModulUtama Translation")]
    public static void SetupModulUtama()
    {
        string scenePath = "Assets/Scenes/ModulUtama.unity";
        var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
        
        GameObject dashboard = GameObject.Find("ParentDashboard");
        if (dashboard != null)
        {
            Transform header = dashboard.transform.Find("Header");
            if (header != null)
            {
                SetTranslationKey(header.Find("Title"), "dash_title");
                SetTranslationKey(header.Find("CloseButton/Text"), "btn_close");
            }
            
            Transform tabs = dashboard.transform.Find("TabContainer");
            if (tabs != null)
            {
                SetTranslationKey(tabs.Find("Tab_Profile/Text"), "tab_profile");
                SetTranslationKey(tabs.Find("Tab_Media/Text"), "tab_media");
                SetTranslationKey(tabs.Find("Tab_Progress/Text"), "tab_progress");
                SetTranslationKey(tabs.Find("Tab_Settings/Text"), "tab_settings");
            }
            
            Transform content = dashboard.transform.Find("ContentArea/ChildProfileContent");
            if (content != null)
            {
                SetTranslationKey(content.Find("Label_Name"), "lbl_child_name");
                SetTranslationKey(content.Find("Label_Age"), "lbl_age");
                SetTranslationKey(content.Find("Label_Avatar"), "lbl_select_avatar");
                SetTranslationKey(content.Find("Btn_SaveProfile/Text"), "btn_save_profile");
            }
        }
        
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
        Debug.Log("3. ModulUtama Translation Setup Complete!");
    }

    private static void SetTranslationKey(Transform obj, string key)
    {
        if (obj == null) return;
        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        if (tmp == null) return;
        
        TeksTerjemahan trans = obj.GetComponent<TeksTerjemahan>();
        if (trans == null) trans = obj.gameObject.AddComponent<TeksTerjemahan>();
        trans.kunciTerjemahan = key;
    }
}
