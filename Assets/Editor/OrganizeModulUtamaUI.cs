using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class OrganizeModulUtamaUI : EditorWindow
{
    [MenuItem("Antigravity/Organize Modul Utama UI")]
    public static void Organize()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        Transform bg = canvas.transform.Find("background");
        if (bg == null)
        {
            Debug.LogError("Canvas/background not found!");
            return;
        }

        // 1. Setup Background Color
        Image bgImage = bg.GetComponent<Image>();
        if (bgImage != null)
        {
            ColorUtility.TryParseHtmlString("#F2F4F7", out Color bgColor);
            bgImage.color = bgColor;
        }

        // 2. Hide old Title and ModButton
        Transform title = bg.Find("Title");
        if (title != null) title.gameObject.SetActive(false);
        Transform modButton = bg.Find("ModButton");
        if (modButton != null) modButton.gameObject.SetActive(false);

        // 3. Create Sidebar
        Transform sidebar = bg.Find("Sidebar");
        if (sidebar != null) DestroyImmediate(sidebar.gameObject);

        GameObject sidebarObj = new GameObject("Sidebar");
        sidebarObj.transform.SetParent(bg, false);
        RectTransform sidebarRect = sidebarObj.AddComponent<RectTransform>();
        sidebarRect.anchorMin = new Vector2(0, 0);
        sidebarRect.anchorMax = new Vector2(0.22f, 1);
        sidebarRect.sizeDelta = Vector2.zero;
        sidebarRect.anchoredPosition = Vector2.zero;

        Image sidebarImage = sidebarObj.AddComponent<Image>();
        ColorUtility.TryParseHtmlString("#1965B2", out Color sidebarColor);
        sidebarImage.color = sidebarColor;

        // Logo Area
        GameObject logoArea = new GameObject("LogoArea");
        logoArea.transform.SetParent(sidebarObj.transform, false);
        RectTransform logoRect = logoArea.AddComponent<RectTransform>();
        logoRect.anchorMin = new Vector2(0, 0.8f);
        logoRect.anchorMax = new Vector2(1, 1);
        logoRect.sizeDelta = Vector2.zero;

        CreateText(logoArea.transform, "BizaraText", "BIZARA", 32, true, Color.white, new Vector2(0, 0.3f), new Vector2(1, 0.8f));
        CreateText(logoArea.transform, "SayaBolehText", "Saya Boleh!", 24, false, Color.white, new Vector2(0, 0), new Vector2(1, 0.3f));

        // Menu Area
        GameObject menuArea = new GameObject("MenuArea");
        menuArea.transform.SetParent(sidebarObj.transform, false);
        RectTransform menuRect = menuArea.AddComponent<RectTransform>();
        menuRect.anchorMin = new Vector2(0, 0);
        menuRect.anchorMax = new Vector2(1, 0.8f);
        menuRect.sizeDelta = Vector2.zero;

        VerticalLayoutGroup menuLayout = menuArea.AddComponent<VerticalLayoutGroup>();
        menuLayout.padding = new RectOffset(20, 20, 20, 20);
        menuLayout.spacing = 15;
        menuLayout.childControlHeight = true;
        menuLayout.childControlWidth = true;
        menuLayout.childForceExpandHeight = false;

        CreateMenuButton(menuArea.transform, "Home", true);
        CreateMenuButton(menuArea.transform, "Eating & Drinking", false);
        CreateMenuButton(menuArea.transform, "Toilet Time", false);
        CreateMenuButton(menuArea.transform, "I Am Clean", false);
        CreateMenuButton(menuArea.transform, "I Am Organized", false);

        // 4. Organize Main Content (Cards)
        Transform cards = bg.Find("Cards");
        if (cards != null)
        {
            RectTransform cardsRect = cards.GetComponent<RectTransform>();
            cardsRect.anchorMin = new Vector2(0.25f, 0.05f);
            cardsRect.anchorMax = new Vector2(0.95f, 0.95f);
            cardsRect.sizeDelta = Vector2.zero;

            // Remove ScrollRect if present to freeze it
            ScrollRect scroll = cards.GetComponent<ScrollRect>();
            if (scroll != null) DestroyImmediate(scroll);

            Transform content = cards.Find("Content");
            if (content != null)
            {
                RectTransform contentRect = content.GetComponent<RectTransform>();
                contentRect.anchorMin = new Vector2(0, 0);
                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.sizeDelta = Vector2.zero;

                HorizontalLayoutGroup hLayout = content.GetComponent<HorizontalLayoutGroup>();
                if (hLayout != null) DestroyImmediate(hLayout);

                GridLayoutGroup grid = content.GetComponent<GridLayoutGroup>();
                if (grid == null) grid = content.gameObject.AddComponent<GridLayoutGroup>();
                
                grid.cellSize = new Vector2(500, 350);
                grid.spacing = new Vector2(50, 50);
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = 2;
                grid.childAlignment = TextAnchor.MiddleCenter;

                // Update the 4 cards
                UpdateCard(content, "Modul1", "Eating & Drinking", "I Can Eat Independently", true);
                UpdateCard(content, "Modul1 (1)", "Toilet Time", "I Can Go To The Toilet", true);
                UpdateCard(content, "Modul1 (2)", "I Am Clean", "I Can Keep Myself Clean", false);
                UpdateCard(content, "Modul1 (3)", "I Am Organized", "I Can Prepare Myself", false);
            }
        }

        EditorUtility.SetDirty(canvas);
        Debug.Log("UI Organized successfully!");
    }

    private static void CreateText(Transform parent, string name, string text, float size, bool isBold, Color color, Vector2 min, Vector2 max)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = isBold ? FontStyles.Bold : FontStyles.Normal;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
    }

    private static void CreateMenuButton(Transform parent, string text, bool isActive)
    {
        GameObject obj = new GameObject("Btn_" + text);
        obj.transform.SetParent(parent, false);
        
        // Add layout element to give button some height
        LayoutElement le = obj.AddComponent<LayoutElement>();
        le.minHeight = 60;

        Image img = obj.AddComponent<Image>();
        ColorUtility.TryParseHtmlString(isActive ? "#F0F4F8" : "#1A5798", out Color bgColor);
        img.color = bgColor;

        CreateText(obj.transform, "Text", text, 20, isActive, isActive ? new Color(0.1f, 0.4f, 0.7f) : Color.white, new Vector2(0, 0), new Vector2(1, 1));
    }

    private static void UpdateCard(Transform content, string objName, string titleStr, string subStr, bool showStars)
    {
        Transform card = content.Find(objName);
        if (card == null) return;

        Image img = card.GetComponent<Image>();
        if (img != null) img.color = Color.white;

        Transform title = card.Find("TitleText");
        if (title != null)
        {
            TextMeshProUGUI tmp = title.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = titleStr;
                tmp.color = new Color(0.2f, 0.2f, 0.2f);
                tmp.fontStyle = FontStyles.Bold;
            }
            RectTransform rect = title.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.4f);
            rect.anchorMax = new Vector2(1, 0.6f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }

        Transform sub = card.Find("subTitle");
        if (sub != null)
        {
            TextMeshProUGUI tmp = sub.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = subStr;
                tmp.color = new Color(0.5f, 0.5f, 0.5f);
            }
            RectTransform rect = sub.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.2f);
            rect.anchorMax = new Vector2(1, 0.4f);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }

        // Add stars if required
        Transform existingStars = card.Find("StarsArea");
        if (showStars && existingStars == null)
        {
            GameObject stars = new GameObject("StarsArea");
            stars.transform.SetParent(card, false);
            RectTransform rect = stars.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 0.2f);
            rect.sizeDelta = Vector2.zero;

            CreateText(stars.transform, "StarsText", "⭐⭐⭐ (9/9)", 18, true, new Color(0.9f, 0.7f, 0.1f), new Vector2(0, 0), new Vector2(1, 1));
        }
        else if (!showStars && existingStars != null)
        {
            DestroyImmediate(existingStars.gameObject);
        }
    }
}
