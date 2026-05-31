using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class SetupBizaraLogo : EditorWindow
{
    [MenuItem("Antigravity/Setup Bizara Logo")]
    public static void SetupLogo()
    {
        string logoPath = "Assets/Sprites/BizaraLogo.png";
        
        // 1. Ensure the asset is imported as a Sprite
        TextureImporter importer = AssetImporter.GetAtPath(logoPath) as TextureImporter;
        if (importer != null)
        {
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
                Debug.Log("Imported BizaraLogo as Sprite.");
            }
        }
        else
        {
            Debug.LogError($"Could not find TextureImporter for {logoPath}. Make sure the file exists and Unity has refreshed.");
            return;
        }

        Sprite logoSprite = AssetDatabase.LoadAssetAtPath<Sprite>(logoPath);
        if (logoSprite == null)
        {
            Debug.LogError("Failed to load BizaraLogo Sprite!");
            return;
        }

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
            if (canvas == null) continue;

            Transform logoArea = canvas.transform.Find("background/Sidebar/LogoArea");
            if (logoArea == null) continue;

            // Hide the text logo
            Transform textLogo = logoArea.Find("BizaraText");
            if (textLogo != null)
            {
                textLogo.gameObject.SetActive(false);
            }

            // Create or update the Logo Image
            Transform logoImgTransform = logoArea.Find("LogoImage");
            GameObject logoImgObj;
            Image img;
            
            if (logoImgTransform == null)
            {
                logoImgObj = new GameObject("LogoImage");
                logoImgObj.transform.SetParent(logoArea, false);
                logoImgObj.transform.SetSiblingIndex(0); // Put it before the "Saya Boleh" text
                img = logoImgObj.AddComponent<Image>();
            }
            else
            {
                logoImgObj = logoImgTransform.gameObject;
                img = logoImgObj.GetComponent<Image>();
            }

            img.sprite = logoSprite;
            img.preserveAspect = true;

            // Set layout
            RectTransform rect = logoImgObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.1f, 0.35f);
            rect.anchorMax = new Vector2(0.9f, 0.95f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Setup logo in {scenePath}");
        }

        // Return to ModulUtama
        EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        Debug.Log("Finished setting up Bizara Logo in all scenes.");
    }
}
