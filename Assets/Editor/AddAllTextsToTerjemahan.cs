using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class AddAllTextsToTerjemahan : EditorWindow
{
    [MenuItem("Antigravity/Translate All Modul Utama Texts")]
    public static void Execute()
    {
        string scenePath = "Assets/Scenes/ModulUtama.unity";
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        
        DataTerjemahan data = AssetDatabase.LoadAssetAtPath<DataTerjemahan>("Assets/Resources/DataTerjemahan.asset");
        if (data == null) {
            Debug.LogError("DataTerjemahan not found!");
            return;
        }

        TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int addedCount = 0;

        // Map exact raw text to (Malay, English)
        Dictionary<string, (string my, string en)> translations = new Dictionary<string, (string, string)>()
        {
            {"Upload", ("Muat Naik", "Upload")},
            {"Upload Image", ("Muat Naik Imej", "Upload Image")},
            {"Upload MP4", ("Muat Naik MP4", "Upload MP4")},
            {"Status", ("Status", "Status")},
            {"MODUL UTAMA", ("MODUL UTAMA", "MAIN MODULE")},
            {"Default instruction video active.", ("Video arahan lalai aktif.", "Default instruction video active.")},
            {"Child Profile", ("Profil Anak", "Child Profile")},
            {"Jumlah Siap", ("Jumlah Siap", "Total Completed")},
            {"Repeated", ("Diulang", "Repeated")},
            {"I Am Clean", ("Saya Bersih", "I Am Clean")},
            {"I Am Organized", ("Saya Kemas", "I Am Organized")},
            {"Select Avatar", ("Pilih Avatar", "Select Avatar")},
            {"System Settings", ("Tetapan Sistem", "System Settings")},
            {"Close", ("Tutup", "Close")},
            {"Module", ("Modul", "Module")},
            {"Date & Time", ("Tarikh & Masa", "Date & Time")},
            {"Saya Boleh!", ("Saya Boleh!", "I Can Do It!")},
            {"Manage Images & Video", ("Urus Imej & Video", "Manage Images & Video")},
            {"Download CSV Report", ("Muat Turun Laporan CSV", "Download CSV Report")},
            {"I Can Go To The Toilet", ("Saya Boleh Ke Tandas", "I Can Go To The Toilet")},
            {"I Can Keep Myself Clean", ("Saya Boleh Jaga Kebersihan", "I Can Keep Myself Clean")},
            {"I Can Prepare Myself", ("Saya Boleh Bersiap Sendiri", "I Can Prepare Myself")},
            {"Toilet Time", ("Masa Tandas", "Toilet Time")},
            {"Parent Mode", ("Mod Ibu Bapa", "Parent Mode")},
            {"I Can Eat Independently", ("Saya Boleh Makan Sendiri", "I Can Eat Independently")},
            {"Name", ("Nama", "Name")},
            {"Eating & Drinking", ("Makan & Minum", "Eating & Drinking")},
            {"Reset All", ("Tetapkan Semula Semua", "Reset All")},
            {"Home", ("Utama", "Home")},
            {"Parent & Teacher Dashboard", ("Papan Pemuka Ibu Bapa & Guru", "Parent & Teacher Dashboard")},
            {"Activity", ("Aktiviti", "Activity")},
            {"Mod Cikgu / Ibubapa", ("Mod Cikgu / Ibubapa", "Teacher / Parent Mode")},
            {"Age (Years)", ("Umur (Tahun)", "Age (Years)")},
            {"English / Melayu", ("English / Melayu", "English / Melayu")},
            {"💾 Save Profile", ("💾 Simpan Profil", "💾 Save Profile")},
            {"Child Name", ("Nama Anak", "Child Name")}
        };

        foreach (var tmp in allTexts)
        {
            string rawText = tmp.text.Trim();
            
            if (string.IsNullOrEmpty(rawText) || rawText.Length <= 1) continue;
            if (rawText.Contains("⭐") || rawText == "Data" || rawText == "Button" || rawText == "New Text" || rawText == "Player Name" || rawText == "BIZARA") continue;

            if (translations.ContainsKey(rawText))
            {
                // Generate a key based on English text (or raw)
                string key = "txt_" + new string(translations[rawText].en.Where(c => char.IsLetterOrDigit(c) || c == ' ').ToArray()).Replace(" ", "_").ToLower();
                if (key.Length > 20) key = key.Substring(0, 20);

                if (!data.senaraiTerjemahan.Exists(x => x.kunci == key))
                {
                    data.senaraiTerjemahan.Add(new KunciTerjemahan { 
                        kunci = key, 
                        teksMelayu = translations[rawText].my, 
                        teksInggeris = translations[rawText].en 
                    });
                }

                TeksTerjemahan tt = tmp.GetComponent<TeksTerjemahan>();
                if (tt == null)
                {
                    tt = tmp.gameObject.AddComponent<TeksTerjemahan>();
                    tt.kunciTerjemahan = key;
                    addedCount++;
                }
                else if (tt.kunciTerjemahan != key)
                {
                    tt.kunciTerjemahan = key;
                    addedCount++;
                }
            }
        }
        
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Successfully added all translations and updated " + addedCount + " text elements in ModulUtama.");
    }
}
