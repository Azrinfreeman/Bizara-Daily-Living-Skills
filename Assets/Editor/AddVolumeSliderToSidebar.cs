using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class AddVolumeSliderToSidebar : EditorWindow
{
    [MenuItem("Antigravity/Add Volume Slider To Sidebar")]
    public static void Execute()
    {
        string prefabPath = "Assets/Prefabs/Sidebar.prefab";
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        Transform menuArea = prefabRoot.transform.Find("MenuArea");
        if (menuArea == null) {
            Debug.LogError("MenuArea not found in Sidebar prefab!");
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return;
        }
        
        Transform bottomSettingsArea = prefabRoot.transform.Find("SettingsArea"); // might be named SettingsArea
        Transform targetParent = bottomSettingsArea != null ? bottomSettingsArea : menuArea;

        Transform existingSlider = targetParent.Find("VolumeSliderArea");
        if (existingSlider != null)
        {
            DestroyImmediate(existingSlider.gameObject);
        }

        // Create Container
        GameObject volumeObj = new GameObject("VolumeSliderArea", typeof(RectTransform));
        volumeObj.transform.SetParent(targetParent, false);
        LayoutElement layoutElement = volumeObj.AddComponent<LayoutElement>();
        layoutElement.minHeight = 60f;
        layoutElement.preferredHeight = 60f;

        // Title Text
        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(volumeObj.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.5f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.sizeDelta = new Vector2(0, 0);
        titleRect.anchoredPosition = new Vector2(0, 0);

        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        titleText.text = "Music Volume";
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.white;
        titleText.fontSize = 18;
        
        // Font
        TextMeshProUGUI anyText = prefabRoot.GetComponentInChildren<TextMeshProUGUI>(true);
        if (anyText != null && anyText.font != null) {
            titleText.font = anyText.font;
            titleText.fontSharedMaterial = anyText.fontSharedMaterial;
        }

        // Slider Object
        // Using standard Unity UI Slider creation method via DefaultControls
        GameObject sliderObj = DefaultControls.CreateSlider(new DefaultControls.Resources());
        sliderObj.transform.SetParent(volumeObj.transform, false);
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.1f, 0);
        sliderRect.anchorMax = new Vector2(0.9f, 0.5f);
        sliderRect.sizeDelta = new Vector2(0, -10);
        sliderRect.anchoredPosition = new Vector2(0, 5);

        Slider slider = sliderObj.GetComponent<Slider>();
        slider.value = 1f;

        // Add our script
        BGMVolumeController bgmController = volumeObj.AddComponent<BGMVolumeController>();
        bgmController.volumeSlider = slider;

        // Place it before the settings buttons if in MenuArea, or at the bottom
        volumeObj.transform.SetAsLastSibling();

        EditorUtility.SetDirty(prefabRoot);
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        Debug.Log("Successfully added Volume Slider to Sidebar!");
    }
}
