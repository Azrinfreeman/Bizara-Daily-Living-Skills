using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AddProfileToSidebar : EditorWindow
{
    [MenuItem("Antigravity/Add Profile To Sidebar")]
    public static void Execute()
    {
        string originalScenePath = EditorSceneManager.GetActiveScene().path;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        // 1. Get Sprites from PaparData in ModulUtama
        string modulUtamaPath = "Assets/Scenes/ModulUtama.unity";
        Scene modulUtama = EditorSceneManager.OpenScene(modulUtamaPath, OpenSceneMode.Single);
        
        PaparData paparData = Object.FindObjectOfType<PaparData>(true);
        if (paparData == null) {
            Debug.LogError("PaparData not found in ModulUtama!");
            return;
        }

        Sprite[] orderedSprites = new Sprite[paparData.avatarOutlines.Length];
        int foundCount = 0;

        for (int i = 0; i < paparData.avatarOutlines.Length; i++) {
            if (paparData.avatarOutlines[i] != null) {
                // The actual avatar image is usually a child named "Icon"
                Transform iconT = paparData.avatarOutlines[i].transform.Find("Icon");
                if (iconT != null) {
                    Image iconImg = iconT.GetComponent<Image>();
                    if (iconImg != null && iconImg.sprite != null) {
                        orderedSprites[i] = iconImg.sprite;
                        foundCount++;
                        continue;
                    }
                }
                
                // Fallback: look for any sprite whose name contains 'avatar'
                Image[] imgs = paparData.avatarOutlines[i].GetComponentsInChildren<Image>(true);
                foreach (Image img in imgs) {
                    if (img.sprite != null && img.sprite.name.ToLower().Contains("avatar")) {
                        orderedSprites[i] = img.sprite;
                        foundCount++;
                        break;
                    }
                }
            }
        }

        Debug.Log("Successfully extracted " + foundCount + " sprites out of " + orderedSprites.Length + " outlines.");

        // 2. Modify Sidebar Prefab
        string prefabPath = "Assets/Prefabs/Sidebar.prefab";
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        Transform menuArea = prefabRoot.transform.Find("MenuArea");
        if (menuArea == null) {
            Debug.LogError("MenuArea not found in Sidebar prefab!");
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return;
        }

        Transform existingProfile = menuArea.Find("PlayerProfile");
        GameObject profileObj;
        ProfilSidebar ps;
        Image avatarImg;
        TextMeshProUGUI nameText;

        if (existingProfile != null) {
            profileObj = existingProfile.gameObject;
            ps = profileObj.GetComponent<ProfilSidebar>();
            avatarImg = profileObj.transform.Find("Avatar").GetComponent<Image>();
            nameText = profileObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        } else {
            profileObj = new GameObject("PlayerProfile", typeof(RectTransform));
            profileObj.transform.SetParent(menuArea, false);
            
            LayoutElement layoutElement = profileObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 120f;
            layoutElement.preferredHeight = 120f;
            
            GameObject avatarObj = new GameObject("Avatar", typeof(RectTransform), typeof(Image));
            avatarObj.transform.SetParent(profileObj.transform, false);
            RectTransform avatarRect = avatarObj.GetComponent<RectTransform>();
            avatarRect.anchorMin = new Vector2(0.5f, 1f);
            avatarRect.anchorMax = new Vector2(0.5f, 1f);
            avatarRect.pivot = new Vector2(0.5f, 1f);
            avatarRect.anchoredPosition = new Vector2(0, -10);
            avatarRect.sizeDelta = new Vector2(70, 70);
            avatarImg = avatarObj.GetComponent<Image>();
            avatarImg.preserveAspect = true;

            GameObject nameObj = new GameObject("NameText", typeof(RectTransform), typeof(TextMeshProUGUI));
            nameObj.transform.SetParent(profileObj.transform, false);
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0);
            nameRect.anchorMax = new Vector2(1, 0);
            nameRect.pivot = new Vector2(0.5f, 0f);
            nameRect.anchoredPosition = new Vector2(0, 5);
            nameRect.sizeDelta = new Vector2(0, 30);

            nameText = nameObj.GetComponent<TextMeshProUGUI>();
            nameText.text = "Player Name";
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.color = Color.white;
            nameText.enableAutoSizing = true;
            nameText.fontSizeMin = 14;
            nameText.fontSizeMax = 24;

            TextMeshProUGUI anyText = menuArea.GetComponentInChildren<TextMeshProUGUI>(true);
            if (anyText != null && anyText.font != null) {
                nameText.font = anyText.font;
                nameText.fontSharedMaterial = anyText.fontSharedMaterial;
            }

            ps = profileObj.AddComponent<ProfilSidebar>();
        }

        profileObj.transform.SetAsLastSibling();

        // Update the sprites
        ps.avatarImage = avatarImg;
        ps.namaText = nameText;
        ps.avatarSprites = orderedSprites;

        if (orderedSprites.Length > 0 && orderedSprites[0] != null) {
            avatarImg.sprite = orderedSprites[0];
        }

        EditorUtility.SetDirty(ps);
        EditorUtility.SetDirty(prefabRoot);

        // Save Prefab
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        if (!string.IsNullOrEmpty(originalScenePath) && originalScenePath != modulUtamaPath) {
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        }

        Debug.Log("Successfully updated PlayerProfile sprites in Sidebar! Count: " + orderedSprites.Length);
    }
}
