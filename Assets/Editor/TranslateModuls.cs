using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TranslateModuls : EditorWindow
{
    [MenuItem("Antigravity/Translate All Moduls 1 to 4")]
    public static void Execute()
    {
        string[] scenes = new string[] { 
            "Assets/Scenes/Modul1.unity", 
            "Assets/Scenes/Modul2.unity", 
            "Assets/Scenes/Modul3.unity", 
            "Assets/Scenes/Modul4.unity" 
        };
        
        DataTerjemahan data = AssetDatabase.LoadAssetAtPath<DataTerjemahan>("Assets/Resources/DataTerjemahan.asset");
        if (data == null) {
            Debug.LogError("DataTerjemahan not found!");
            return;
        }

        Dictionary<string, (string my, string en)> translations = new Dictionary<string, (string, string)>()
        {
            {"Bagus", ("Bagus", "Good")},
            {"Susun langkah sebelum makan", ("Susun langkah sebelum makan", "Arrange the steps before eating")},
            {"MAKAN DAN MINUM", ("MAKAN DAN MINUM", "EATING AND DRINKING")},
            {"NEXT", ("SETERUSNYA", "NEXT")},
            {"VIDEO PANDUAN", ("VIDEO PANDUAN", "GUIDE VIDEO")},
            {"Adam pegang sudu.", ("Adam pegang sudu.", "Adam holds the spoon.")},
            {"Letak sudu ke pinggan.", ("Letak sudu ke pinggan.", "Put the spoon on the plate.")},
            {"PILIH SUDU", ("PILIH SUDU", "CHOOSE SPOON")},
            {"Apa perlu buat selepas guna tandas?", ("Apa perlu buat selepas guna tandas?", "What to do after using the toilet?")},
            {"Adam mahu ke tandas", ("Adam mahu ke tandas", "Adam wants to go to the toilet")},
            {"Gerakkan tisu ke mangkuk tandas", ("Gerakkan tisu ke mangkuk tandas", "Move the tissue to the toilet bowl")},
            {"Adam Boleh Jaga Kesihatan", ("Adam Boleh Jaga Kesihatan", "Adam Can Stay Healthy")},
            {"gerakkan tangan ke sabun", ("gerakkan tangan ke sabun", "Move hand to the soap")},
            {"Yang manakah bersih", ("Yang manakah bersih", "Which one is clean")},
            {"Susun langkah basuh tangan", ("Susun langkah basuh tangan", "Arrange the hand washing steps")},
            {"Susun langkah kemas barang", ("Susun langkah kemas barang", "Arrange the steps to organize items")},
            {"Adam mahu bersiap", ("Adam mahu bersiap", "Adam wants to get ready")},
            {"Kemaskan barang ke dalam beg", ("Kemaskan barang ke dalam beg", "Pack the items into the bag")},
            {"Pilih Kasut", ("Pilih Kasut", "Choose Shoes")},
            {"HOME", ("LAMAN UTAMA", "HOME")},
            {"TRY AGAIN", ("CUBA LAGI", "TRY AGAIN")},
            
            // Re-include common ones just in case
            {"Upload", ("Muat Naik", "Upload")},
            {"Toilet Time", ("Masa Tandas", "Toilet Time")},
            {"Saya Boleh!", ("Saya Boleh!", "I Can Do It!")},
            {"I Am Organized", ("Saya Kemas", "I Am Organized")},
            {"Eating & Drinking", ("Makan & Minum", "Eating & Drinking")},
            {"Home", ("Utama", "Home")},
            {"I Am Clean", ("Saya Bersih", "I Am Clean")}
        };

        int totalAddedCount = 0;

        foreach (string path in scenes)
        {
            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            int addedCount = 0;

            foreach (var tmp in allTexts)
            {
                string rawText = tmp.text.Trim();
                
                if (string.IsNullOrEmpty(rawText) || rawText.Length <= 1) continue;
                if (rawText.Contains("⭐") || rawText == "Data" || rawText == "Button" || rawText == "New Text" || rawText == "Player Name" || rawText == "BIZARA") continue;

                if (translations.ContainsKey(rawText))
                {
                    string key = "txt_" + new string(translations[rawText].en.Where(c => char.IsLetterOrDigit(c) || c == ' ').ToArray()).Replace(" ", "_").ToLower();
                    if (key.Length > 25) key = key.Substring(0, 25);

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
            
            EditorSceneManager.SaveScene(scene);
            totalAddedCount += addedCount;
        }

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully attached TeksTerjemahan to " + totalAddedCount + " elements across Modul 1-4.");
    }
}
