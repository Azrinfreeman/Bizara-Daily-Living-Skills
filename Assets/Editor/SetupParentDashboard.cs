using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using TMPro;

public class SetupParentDashboard : EditorWindow
{
    [MenuItem("Antigravity/Setup Parent Dashboard")]
    public static void Setup()
    {
        EnsureSpritesImported();

        var scene = EditorSceneManager.OpenScene("Assets/Scenes/ModulUtama.unity", OpenSceneMode.Single);
        
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in ModulUtama.");
            return;
        }

        Transform[] allTransforms = canvas.GetComponentsInChildren<Transform>(true);
        Transform panelModIbuBapa = null;
        foreach (Transform t in allTransforms)
        {
            if (t.name == "Panel_ModIbuBapa")
            {
                panelModIbuBapa = t;
                break;
            }
        }

        if (panelModIbuBapa == null)
        {
            Debug.LogError("Panel_ModIbuBapa not found.");
            return;
        }

        // Get or add PaparData and TabManager
        PaparData paparData = panelModIbuBapa.GetComponent<PaparData>();
        if (paparData == null) paparData = panelModIbuBapa.gameObject.AddComponent<PaparData>();

        TabManager tabManager = panelModIbuBapa.GetComponent<TabManager>();
        if (tabManager == null) tabManager = panelModIbuBapa.gameObject.AddComponent<TabManager>();

        // Clear existing children
        for (int i = panelModIbuBapa.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(panelModIbuBapa.GetChild(i).gameObject);
        }

        // 1. Panel Background Setup
        RectTransform panelRect = panelModIbuBapa.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(1000, 650);
        }

        Image panelBg = panelModIbuBapa.GetComponent<Image>();
        if (panelBg == null) panelBg = panelModIbuBapa.gameObject.AddComponent<Image>();
        
        ColorUtility.TryParseHtmlString("#E8ECF1", out Color bgColor);
        panelBg.color = bgColor;
        panelBg.type = Image.Type.Sliced;

        string darkBlueStr = "#2C3E50";
        string activeBlueStr = "#1965B2";
        string borderBlueStr = "#3482D2";
        string tabGrayStr = "#7F8C8D";
        
        ColorUtility.TryParseHtmlString(darkBlueStr, out Color darkBlue);
        ColorUtility.TryParseHtmlString(activeBlueStr, out Color activeBlue);
        ColorUtility.TryParseHtmlString(borderBlueStr, out Color borderBlue);
        ColorUtility.TryParseHtmlString(tabGrayStr, out Color tabGray);

        // Load Sprites
        Sprite houseSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_house.png");
        Sprite toolsSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_tools.png");
        Sprite closeSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_close.png");
        
        Sprite folderSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_folder.png");
        Sprite chartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_chart.png");
        Sprite gearSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_gear.png");
        Sprite profileSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/avatar_profile.png");

        Sprite downloadSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_download.png");
        Sprite trashSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_trash.png");
        Sprite starSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/icon_star.png");
        paparData.starSprite = starSprite;

        // 2. Header Section
        GameObject header = new GameObject("Header", typeof(RectTransform));
        header.transform.SetParent(panelModIbuBapa, false);
        RectTransform headerRect = header.GetComponent<RectTransform>();
        headerRect.anchorMin = new Vector2(0, 1);
        headerRect.anchorMax = new Vector2(1, 1);
        headerRect.pivot = new Vector2(0.5f, 1);
        headerRect.anchoredPosition = new Vector2(0, -30);
        headerRect.sizeDelta = new Vector2(-60, 60);

        // Title Holder
        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        titleObj.transform.SetParent(header.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = Vector2.zero;
        
        HorizontalLayoutGroup titleHlg = titleObj.GetComponent<HorizontalLayoutGroup>();
        titleHlg.childAlignment = TextAnchor.MiddleLeft;
        titleHlg.childControlWidth = false;
        titleHlg.childControlHeight = false;
        titleHlg.spacing = 10;

        CreateIcon(titleObj.transform, houseSprite, "HouseIcon", 40);
        CreateIcon(titleObj.transform, toolsSprite, "ToolsIcon", 40);

        GameObject titleTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleTextObj.transform.SetParent(titleObj.transform, false);
        TextMeshProUGUI titleText = titleTextObj.GetComponent<TextMeshProUGUI>();
        titleText.text = "Parent & Teacher Dashboard";
        titleText.fontSize = 32;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = darkBlue;
        titleText.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Midline;
        titleText.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 50);

        // Close Button
        GameObject closeBtnObj = new GameObject("Btn_Close", typeof(RectTransform), typeof(Image), typeof(Button), typeof(HorizontalLayoutGroup));
        closeBtnObj.transform.SetParent(header.transform, false);
        RectTransform closeRect = closeBtnObj.GetComponent<RectTransform>();
        closeRect.anchorMin = new Vector2(1, 0.5f);
        closeRect.anchorMax = new Vector2(1, 0.5f);
        closeRect.pivot = new Vector2(1, 0.5f);
        closeRect.anchoredPosition = Vector2.zero;
        closeRect.sizeDelta = new Vector2(150, 50);

        Image closeBg = closeBtnObj.GetComponent<Image>();
        closeBg.color = bgColor;
        Outline closeOutline = closeBtnObj.AddComponent<Outline>();
        closeOutline.effectColor = activeBlue;
        closeOutline.effectDistance = new Vector2(2, -2);
        
        HorizontalLayoutGroup closeHlg = closeBtnObj.GetComponent<HorizontalLayoutGroup>();
        closeHlg.childAlignment = TextAnchor.MiddleCenter;
        closeHlg.childControlWidth = false;
        closeHlg.childControlHeight = false;
        closeHlg.spacing = 5;

        CreateIcon(closeBtnObj.transform, closeSprite, "CloseIcon", 24);

        GameObject closeTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        closeTextObj.transform.SetParent(closeBtnObj.transform, false);
        TextMeshProUGUI closeText = closeTextObj.GetComponent<TextMeshProUGUI>();
        closeText.text = "Close";
        closeText.fontSize = 24;
        closeText.fontStyle = FontStyles.Bold;
        closeText.color = darkBlue;
        closeText.alignment = TextAlignmentOptions.Center;
        closeText.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 40);

        // 3. Tabs Section
        GameObject tabsContainer = new GameObject("TabsContainer", typeof(RectTransform));
        tabsContainer.transform.SetParent(panelModIbuBapa, false);
        RectTransform tabsRect = tabsContainer.GetComponent<RectTransform>();
        tabsRect.anchorMin = new Vector2(0, 1);
        tabsRect.anchorMax = new Vector2(1, 1);
        tabsRect.pivot = new Vector2(0.5f, 1);
        tabsRect.anchoredPosition = new Vector2(0, -110);
        tabsRect.sizeDelta = new Vector2(-60, 50);

        GameObject separator = new GameObject("Separator", typeof(RectTransform), typeof(Image));
        separator.transform.SetParent(tabsContainer.transform, false);
        RectTransform sepRect = separator.GetComponent<RectTransform>();
        sepRect.anchorMin = new Vector2(0, 0);
        sepRect.anchorMax = new Vector2(1, 0);
        sepRect.pivot = new Vector2(0.5f, 0);
        sepRect.anchoredPosition = new Vector2(0, -5);
        sepRect.sizeDelta = new Vector2(0, 4);
        Image sepImg = separator.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#BDC3C7", out Color sepColor);
        sepImg.color = sepColor;

        GameObject tabLayout = new GameObject("TabLayout", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        tabLayout.transform.SetParent(tabsContainer.transform, false);
        RectTransform tabLayoutRect = tabLayout.GetComponent<RectTransform>();
        tabLayoutRect.anchorMin = Vector2.zero;
        tabLayoutRect.anchorMax = Vector2.one;
        tabLayoutRect.sizeDelta = Vector2.zero;
        
        HorizontalLayoutGroup hlg = tabLayout.GetComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.spacing = 10;

        string[] tabNames = { "Child Profile", "Manage Images & Video", "Progress & CSV", "System Settings" };
        Sprite[] tabIcons = { profileSprite, folderSprite, chartSprite, gearSprite };
        
        Button[] tabButtons = new Button[tabNames.Length];
        Image[] tabUnderlines = new Image[tabNames.Length];
        GameObject[] tabContents = new GameObject[tabNames.Length];

        for (int i = 0; i < tabNames.Length; i++)
        {
            GameObject tab = new GameObject("Tab_" + i, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(Image), typeof(Button));
            tab.transform.SetParent(tabLayout.transform, false);
            
            tab.GetComponent<Image>().color = new Color(0,0,0,0); // invisible background for clicking
            tabButtons[i] = tab.GetComponent<Button>();

            HorizontalLayoutGroup thlg = tab.GetComponent<HorizontalLayoutGroup>();
            thlg.childAlignment = TextAnchor.MiddleCenter;
            thlg.childControlWidth = false;
            thlg.childControlHeight = false;
            thlg.spacing = 5;

            CreateIcon(tab.transform, tabIcons[i], "Icon", 24);

            GameObject tabTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            tabTextObj.transform.SetParent(tab.transform, false);
            TextMeshProUGUI tabText = tabTextObj.GetComponent<TextMeshProUGUI>();
            tabText.text = tabNames[i];
            tabText.fontSize = 20;
            tabText.alignment = TextAlignmentOptions.Center;
            tabText.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 40);
            
            GameObject underline = new GameObject("Underline", typeof(RectTransform), typeof(Image));
            underline.transform.SetParent(tab.transform, false);
            RectTransform ulRect = underline.GetComponent<RectTransform>();
            ulRect.anchorMin = new Vector2(0, 0);
            ulRect.anchorMax = new Vector2(1, 0);
            ulRect.pivot = new Vector2(0.5f, 0);
            ulRect.anchoredPosition = new Vector2(0, -5);
            ulRect.sizeDelta = new Vector2(0, 4);
            
            LayoutElement le = underline.AddComponent<LayoutElement>();
            le.ignoreLayout = true;

            Image ulImg = underline.GetComponent<Image>();
            ulImg.color = activeBlue;
            tabUnderlines[i] = ulImg;
            
            if (i == 0)
            {
                tabText.color = activeBlue;
                tabText.fontStyle = FontStyles.Bold;
                ulImg.enabled = true;
            }
            else
            {
                tabText.color = tabGray;
                tabText.fontStyle = FontStyles.Normal;
                ulImg.enabled = false;
            }
        }

        // 4. Content Area - Child Profile
        GameObject contentChildProfile = new GameObject("Content_ChildProfile", typeof(RectTransform));
        contentChildProfile.transform.SetParent(panelModIbuBapa, false);
        RectTransform contentRect = contentChildProfile.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.offsetMin = new Vector2(30, 30);
        contentRect.offsetMax = new Vector2(-30, -180);
        
        tabContents[0] = contentChildProfile;

        paparData.inputNamaPelajar = CreateInputField(contentChildProfile.transform, "Child Name", "", 0, darkBlue, borderBlue);
        paparData.inputUmurPelajar = CreateInputField(contentChildProfile.transform, "Age (Years)", "", -90, darkBlue, borderBlue);

        GameObject avatarLabelObj = new GameObject("Label_Avatar", typeof(RectTransform), typeof(TextMeshProUGUI));
        avatarLabelObj.transform.SetParent(contentChildProfile.transform, false);
        RectTransform avatarLabelRect = avatarLabelObj.GetComponent<RectTransform>();
        avatarLabelRect.anchorMin = new Vector2(0, 1);
        avatarLabelRect.anchorMax = new Vector2(1, 1);
        avatarLabelRect.pivot = new Vector2(0.5f, 1);
        avatarLabelRect.anchoredPosition = new Vector2(0, -180);
        avatarLabelRect.sizeDelta = new Vector2(0, 30);
        
        TextMeshProUGUI avatarLabel = avatarLabelObj.GetComponent<TextMeshProUGUI>();
        avatarLabel.text = "Select Avatar";
        avatarLabel.fontSize = 20;
        avatarLabel.fontStyle = FontStyles.Bold;
        avatarLabel.color = darkBlue;

        GameObject avatarGrid = new GameObject("AvatarGrid", typeof(RectTransform), typeof(GridLayoutGroup));
        avatarGrid.transform.SetParent(contentChildProfile.transform, false);
        RectTransform gridRect = avatarGrid.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0, 1);
        gridRect.anchorMax = new Vector2(1, 1);
        gridRect.pivot = new Vector2(0, 1);
        gridRect.anchoredPosition = new Vector2(0, -220);
        gridRect.sizeDelta = new Vector2(0, 200);

        GridLayoutGroup glg = avatarGrid.GetComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(60, 60);
        glg.spacing = new Vector2(15, 15);
        glg.startCorner = GridLayoutGroup.Corner.UpperLeft;
        glg.startAxis = GridLayoutGroup.Axis.Horizontal;
        glg.childAlignment = TextAnchor.UpperLeft;
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = 8;

        string[] avatarNames = { "profile", "boy", "girl", "kid", "lion", "fox", "koala", "panda" };
        paparData.avatarOutlines = new Outline[avatarNames.Length];
        
        for (int i = 0; i < avatarNames.Length; i++)
        {
            GameObject avatarBtn = new GameObject("Avatar_" + i, typeof(RectTransform), typeof(Image), typeof(Button));
            avatarBtn.transform.SetParent(avatarGrid.transform, false);
            
            Image avBg = avatarBtn.GetComponent<Image>();
            ColorUtility.TryParseHtmlString("#F8F9FA", out Color avColor);
            avBg.color = avColor;

            Outline avOutline = avatarBtn.AddComponent<Outline>();
            avOutline.effectColor = activeBlue;
            avOutline.effectDistance = new Vector2(3, -3);
            avOutline.enabled = false; 
            paparData.avatarOutlines[i] = avOutline;

            Button btn = avatarBtn.GetComponent<Button>();
            int index = i; 
            
            UnityEngine.Events.UnityAction<int> targetAction = new UnityEngine.Events.UnityAction<int>(paparData.PilihAvatar);
            UnityEditor.Events.UnityEventTools.AddIntPersistentListener(btn.onClick, targetAction, index);

            Sprite avSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/DashboardIcons/avatar_" + avatarNames[i] + ".png");
            
            GameObject iconObj = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            iconObj.transform.SetParent(avatarBtn.transform, false);
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchorMin = Vector2.zero;
            iconRect.anchorMax = Vector2.one;
            iconRect.sizeDelta = Vector2.zero;
            
            Image iconImg = iconObj.GetComponent<Image>();
            iconImg.sprite = avSprite;
            iconImg.preserveAspect = true;
        }

        // Add Save Button at the bottom
        GameObject saveBtnObj = new GameObject("Btn_SaveProfile", typeof(RectTransform), typeof(Image), typeof(Button));
        saveBtnObj.transform.SetParent(contentChildProfile.transform, false);
        RectTransform saveBtnRect = saveBtnObj.GetComponent<RectTransform>();
        saveBtnRect.anchorMin = new Vector2(0, 0);
        saveBtnRect.anchorMax = new Vector2(0, 0);
        saveBtnRect.pivot = new Vector2(0, 0);
        saveBtnRect.anchoredPosition = new Vector2(0, -60); // below the grid area
        saveBtnRect.sizeDelta = new Vector2(200, 50);

        Image saveBg = saveBtnObj.GetComponent<Image>();
        saveBg.color = activeBlue;
        
        Button saveBtn = saveBtnObj.GetComponent<Button>();
        paparData.butangSimpan = saveBtn;

        UnityEngine.Events.UnityAction saveAction = new UnityEngine.Events.UnityAction(paparData.SimpanDataProfil);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(saveBtn.onClick, saveAction);

        GameObject saveTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        saveTextObj.transform.SetParent(saveBtnObj.transform, false);
        RectTransform saveTextRect = saveTextObj.GetComponent<RectTransform>();
        saveTextRect.anchorMin = Vector2.zero;
        saveTextRect.anchorMax = Vector2.one;
        saveTextRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI saveText = saveTextObj.GetComponent<TextMeshProUGUI>();
        saveText.text = "💾 Save Profile";
        saveText.fontSize = 22;
        saveText.fontStyle = FontStyles.Bold;
        saveText.color = Color.white;
        saveText.alignment = TextAlignmentOptions.Center;

        // 4b. Content Area - Manage Images & Video
        GameObject contentManageMedia = new GameObject("Content_ManageImagesVideo", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        contentManageMedia.transform.SetParent(panelModIbuBapa, false);
        RectTransform mediaRect = contentManageMedia.GetComponent<RectTransform>();
        mediaRect.anchorMin = new Vector2(0, 0);
        mediaRect.anchorMax = new Vector2(1, 1);
        mediaRect.offsetMin = new Vector2(30, 30);
        mediaRect.offsetMax = new Vector2(-30, -180);
        
        HorizontalLayoutGroup mediaHlg = contentManageMedia.GetComponent<HorizontalLayoutGroup>();
        mediaHlg.spacing = 20;
        mediaHlg.childControlWidth = true;
        mediaHlg.childControlHeight = true;
        mediaHlg.childForceExpandWidth = true;
        mediaHlg.childForceExpandHeight = true;

        tabContents[1] = contentManageMedia;


        PengurusMediaDashboard pengurusMedia = contentManageMedia.AddComponent<PengurusMediaDashboard>();

        // Left Panel (Dashboard)
        GameObject leftPanel = new GameObject("LeftPanel_Dashboard", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(UnityEngine.UI.Outline));
        leftPanel.transform.SetParent(contentManageMedia.transform, false);
        ColorUtility.TryParseHtmlString("#EAF2F8", out Color lpColor);
        leftPanel.GetComponent<Image>().color = lpColor;
        leftPanel.GetComponent<UnityEngine.UI.Outline>().effectColor = activeBlue;
        
        VerticalLayoutGroup leftVlg = leftPanel.GetComponent<VerticalLayoutGroup>();
        leftVlg.padding = new RectOffset(30, 30, 50, 30);
        leftVlg.spacing = 20;
        leftVlg.childAlignment = TextAnchor.UpperCenter;
        leftVlg.childControlWidth = true;
        leftVlg.childControlHeight = false;
        leftVlg.childForceExpandHeight = false;

        GameObject dashBox = new GameObject("DashBox", typeof(RectTransform), typeof(Image));
        dashBox.transform.SetParent(leftPanel.transform, false);
        dashBox.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250);
        ColorUtility.TryParseHtmlString("#D5D8DC", out Color dbColor);
        dashBox.GetComponent<Image>().color = dbColor;
        
        GameObject dashStatusObj = new GameObject("StatusText", typeof(RectTransform), typeof(TextMeshProUGUI));
        dashStatusObj.transform.SetParent(dashBox.transform, false);
        RectTransform dsRect = dashStatusObj.GetComponent<RectTransform>();
        dsRect.anchorMin = Vector2.zero; dsRect.anchorMax = Vector2.one; dsRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI dashStatusText = dashStatusObj.GetComponent<TextMeshProUGUI>();
        dashStatusText.text = "Default instruction video active.";
        dashStatusText.fontSize = 20;
        dashStatusText.fontStyle = FontStyles.Italic;
        dashStatusText.alignment = TextAlignmentOptions.Center;
        ColorUtility.TryParseHtmlString("#7F8C8D", out Color dtColor);
        dashStatusText.color = dtColor;
        pengurusMedia.dashboardStatusText = dashStatusText;

        GameObject btnUploadDashObj = new GameObject("Btn_UploadDashboardMP4", typeof(RectTransform), typeof(Image), typeof(Button));
        btnUploadDashObj.transform.SetParent(leftPanel.transform, false);
        btnUploadDashObj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60);
        btnUploadDashObj.GetComponent<Image>().color = activeBlue;
        Button btnUploadDash = btnUploadDashObj.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btnUploadDash.onClick, new UnityEngine.Events.UnityAction(pengurusMedia.MuatNaikVideoDashboard));

        GameObject btnUploadDashTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        btnUploadDashTextObj.transform.SetParent(btnUploadDashObj.transform, false);
        RectTransform bdtRect = btnUploadDashTextObj.GetComponent<RectTransform>();
        bdtRect.anchorMin = Vector2.zero; bdtRect.anchorMax = Vector2.one; bdtRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI btnUploadDashText = btnUploadDashTextObj.GetComponent<TextMeshProUGUI>();
        btnUploadDashText.text = "Upload MP4";
        btnUploadDashText.fontSize = 22;
        btnUploadDashText.fontStyle = FontStyles.Bold;
        btnUploadDashText.color = Color.white;
        btnUploadDashText.alignment = TextAlignmentOptions.Center;

        // Right Panel (Modules)
        GameObject rightPanel = new GameObject("RightPanel_Modules", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(UnityEngine.UI.Outline));
        rightPanel.transform.SetParent(contentManageMedia.transform, false);
        ColorUtility.TryParseHtmlString("#EAF2F8", out Color rpColor);
        rightPanel.GetComponent<Image>().color = rpColor;
        rightPanel.GetComponent<UnityEngine.UI.Outline>().effectColor = activeBlue;

        VerticalLayoutGroup rightVlg = rightPanel.GetComponent<VerticalLayoutGroup>();
        rightVlg.padding = new RectOffset(30, 30, 30, 30);
        rightVlg.spacing = 20;
        rightVlg.childAlignment = TextAnchor.UpperCenter;
        rightVlg.childControlWidth = true;
        rightVlg.childControlHeight = false;
        rightVlg.childForceExpandHeight = false;

        UnityEngine.UI.DefaultControls.Resources uiRes = new UnityEngine.UI.DefaultControls.Resources();
#if UNITY_EDITOR
        uiRes.standard = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        uiRes.background = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        uiRes.dropdown = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");
        uiRes.checkmark = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
        uiRes.mask = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
#endif

        GameObject dropdownObj = UnityEngine.UI.DefaultControls.CreateDropdown(uiRes);
        dropdownObj.name = "ModuleDropdown";
        dropdownObj.transform.SetParent(rightPanel.transform, false);
        dropdownObj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60);
        LayoutElement ddLe = dropdownObj.AddComponent<LayoutElement>();
        ddLe.preferredHeight = 60;
        
        Dropdown moduleDropdown = dropdownObj.GetComponent<Dropdown>();
        
        Transform labelTrans = dropdownObj.transform.Find("Label");
        if (labelTrans != null) {
            Text labelText = labelTrans.GetComponent<Text>();
            labelText.fontSize = 20;
            labelText.color = Color.black;
            labelTrans.GetComponent<RectTransform>().offsetMin = new Vector2(15, 0);
        }

        Transform templateTrans = dropdownObj.transform.Find("Template");
        if (templateTrans != null) {
            templateTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 350);
            Transform itemTrans = templateTrans.Find("Viewport/Content/Item");
            if (itemTrans != null) {
                itemTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60);
                Transform itemLabelTrans = itemTrans.Find("Item Label");
                if (itemLabelTrans != null) {
                    Text itemLabelText = itemLabelTrans.GetComponent<Text>();
                    itemLabelText.fontSize = 20;
                    itemLabelText.color = Color.black;
                    itemLabelTrans.GetComponent<RectTransform>().offsetMin = new Vector2(15, 0);
                }
                Transform checkmarkTrans = itemTrans.Find("Item Checkmark");
                if (checkmarkTrans != null) checkmarkTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
            }
        }
        moduleDropdown.options.Clear();
        moduleDropdown.options.Add(new Dropdown.OptionData("Module 1: Eating & Drinking (In-Game)"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Module 2: Toilet Time (In-Game)"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Module 3: I Am Clean (In-Game)"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Module 4: I Am Organized (In-Game)"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Main Menu: Modul 1 Icon"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Main Menu: Modul 2 Icon"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Main Menu: Modul 3 Icon"));
        moduleDropdown.options.Add(new Dropdown.OptionData("Main Menu: Modul 4 Icon"));
        pengurusMedia.modulDropdown = moduleDropdown;

        GameObject previewBox = new GameObject("PreviewBox", typeof(RectTransform), typeof(Image), typeof(Mask));
        previewBox.transform.SetParent(rightPanel.transform, false);
        previewBox.GetComponent<Image>().color = Color.black;
        LayoutElement pbLe = previewBox.AddComponent<LayoutElement>();
        pbLe.preferredHeight = 250;
        pbLe.flexibleHeight = 1;

        GameObject previewImgObj = new GameObject("PreviewImage", typeof(RectTransform), typeof(RawImage), typeof(AspectRatioFitter));
        previewImgObj.transform.SetParent(previewBox.transform, false);
        RectTransform piRect = previewImgObj.GetComponent<RectTransform>();
        piRect.anchorMin = Vector2.zero; piRect.anchorMax = Vector2.one; piRect.sizeDelta = Vector2.zero;
        RawImage previewImage = previewImgObj.GetComponent<RawImage>();
        AspectRatioFitter arf = previewImgObj.GetComponent<AspectRatioFitter>();
        arf.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        arf.aspectRatio = 1.33f;
        pengurusMedia.previewImage = previewImage;

        GameObject rightBtnRow = new GameObject("ButtonRow", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        rightBtnRow.transform.SetParent(rightPanel.transform, false);
        rightBtnRow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60);
        HorizontalLayoutGroup rbrHlg = rightBtnRow.GetComponent<HorizontalLayoutGroup>();
        rbrHlg.spacing = 20;
        rbrHlg.childControlWidth = true;
        rbrHlg.childForceExpandWidth = true;

        // Upload Image Btn
        GameObject btnUploadImgObj = new GameObject("Btn_UploadImage", typeof(RectTransform), typeof(Image), typeof(Button));
        btnUploadImgObj.transform.SetParent(rightBtnRow.transform, false);
        btnUploadImgObj.GetComponent<Image>().color = activeBlue;
        Button btnUploadImg = btnUploadImgObj.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btnUploadImg.onClick, new UnityEngine.Events.UnityAction(pengurusMedia.MuatNaikGambarModul));

        GameObject btnUploadImgTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        btnUploadImgTextObj.transform.SetParent(btnUploadImgObj.transform, false);
        RectTransform bitRect = btnUploadImgTextObj.GetComponent<RectTransform>();
        bitRect.anchorMin = Vector2.zero; bitRect.anchorMax = Vector2.one; bitRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI btnUploadImgText = btnUploadImgTextObj.GetComponent<TextMeshProUGUI>();
        btnUploadImgText.text = "Upload Image";
        btnUploadImgText.fontSize = 22;
        btnUploadImgText.fontStyle = FontStyles.Bold;
        btnUploadImgText.color = Color.white;
        btnUploadImgText.alignment = TextAlignmentOptions.Center;

        // Upload MP4 Btn
        GameObject btnUploadVidObj = new GameObject("Btn_UploadMP4", typeof(RectTransform), typeof(Image), typeof(Button));
        btnUploadVidObj.transform.SetParent(rightBtnRow.transform, false);
        ColorUtility.TryParseHtmlString("#34495E", out Color uvColor);
        btnUploadVidObj.GetComponent<Image>().color = uvColor;
        Button btnUploadVid = btnUploadVidObj.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btnUploadVid.onClick, new UnityEngine.Events.UnityAction(pengurusMedia.MuatNaikVideoModul));

        GameObject btnUploadVidTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        btnUploadVidTextObj.transform.SetParent(btnUploadVidObj.transform, false);
        RectTransform bvtRect = btnUploadVidTextObj.GetComponent<RectTransform>();
        bvtRect.anchorMin = Vector2.zero; bvtRect.anchorMax = Vector2.one; bvtRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI btnUploadVidText = btnUploadVidTextObj.GetComponent<TextMeshProUGUI>();
        btnUploadVidText.text = "Upload MP4";
        btnUploadVidText.fontSize = 22;
        btnUploadVidText.fontStyle = FontStyles.Bold;
        btnUploadVidText.color = Color.white;
        btnUploadVidText.alignment = TextAlignmentOptions.Center;

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentManageMedia.GetComponent<RectTransform>());
        contentManageMedia.SetActive(false);

        // 5. Content Area - Progress & CSV
        GameObject contentProgressCSV = new GameObject("Content_ProgressCSV", typeof(RectTransform));
        contentProgressCSV.transform.SetParent(panelModIbuBapa, false);
        RectTransform progRect = contentProgressCSV.GetComponent<RectTransform>();
        progRect.anchorMin = new Vector2(0, 0);
        progRect.anchorMax = new Vector2(1, 1);
        progRect.offsetMin = new Vector2(30, 30);
        progRect.offsetMax = new Vector2(-30, -180);
        
        tabContents[2] = contentProgressCSV;
        contentProgressCSV.SetActive(false);

        // Progress Top Bar
        GameObject progTopBar = new GameObject("TopBar", typeof(RectTransform));
        progTopBar.transform.SetParent(contentProgressCSV.transform, false);
        RectTransform topBarRect = progTopBar.GetComponent<RectTransform>();
        topBarRect.anchorMin = new Vector2(0, 1);
        topBarRect.anchorMax = new Vector2(1, 1);
        topBarRect.pivot = new Vector2(0, 1);
        topBarRect.anchoredPosition = new Vector2(0, 0);
        topBarRect.sizeDelta = new Vector2(0, 50);

        // Download Button
        GameObject downloadBtnObj = new GameObject("Btn_Download", typeof(RectTransform), typeof(Image), typeof(Button), typeof(HorizontalLayoutGroup));
        downloadBtnObj.transform.SetParent(progTopBar.transform, false);
        RectTransform dRect = downloadBtnObj.GetComponent<RectTransform>();
        dRect.anchorMin = new Vector2(0, 0.5f);
        dRect.anchorMax = new Vector2(0, 0.5f);
        dRect.pivot = new Vector2(0, 0.5f);
        dRect.anchoredPosition = new Vector2(0, 0);
        dRect.sizeDelta = new Vector2(250, 50);

        Image dImg = downloadBtnObj.GetComponent<Image>();
        dImg.color = activeBlue;
        
        HorizontalLayoutGroup dHlg = downloadBtnObj.GetComponent<HorizontalLayoutGroup>();
        dHlg.childAlignment = TextAnchor.MiddleCenter;
        dHlg.childControlWidth = false;
        dHlg.childControlHeight = false;
        dHlg.spacing = 10;
        
        CreateIcon(downloadBtnObj.transform, downloadSprite, "Icon", 24);
        
        GameObject dTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        dTextObj.transform.SetParent(downloadBtnObj.transform, false);
        TextMeshProUGUI dText = dTextObj.GetComponent<TextMeshProUGUI>();
        dText.text = "Download CSV Report";
        dText.fontSize = 20;
        dText.fontStyle = FontStyles.Bold;
        dText.color = Color.white;
        dText.alignment = TextAlignmentOptions.Center;
        dText.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 40);
        
        paparData.butangDownload = downloadBtnObj.GetComponent<Button>();
        UnityEngine.Events.UnityAction dlAction = new UnityEngine.Events.UnityAction(paparData.EksportKeExcel);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(paparData.butangDownload.onClick, dlAction);

        // Reset All Button
        GameObject resetBtnObj = new GameObject("Btn_Reset", typeof(RectTransform), typeof(Image), typeof(Button), typeof(HorizontalLayoutGroup));
        resetBtnObj.transform.SetParent(progTopBar.transform, false);
        RectTransform rRect = resetBtnObj.GetComponent<RectTransform>();
        rRect.anchorMin = new Vector2(0, 0.5f);
        rRect.anchorMax = new Vector2(0, 0.5f);
        rRect.pivot = new Vector2(0, 0.5f);
        rRect.anchoredPosition = new Vector2(270, 0);
        rRect.sizeDelta = new Vector2(150, 50);

        Image rImg = resetBtnObj.GetComponent<Image>();
        rImg.color = bgColor;
        Outline rOutline = resetBtnObj.AddComponent<Outline>();
        ColorUtility.TryParseHtmlString("#E74C3C", out Color redColor);
        rOutline.effectColor = redColor;
        rOutline.effectDistance = new Vector2(2, -2);
        
        HorizontalLayoutGroup rHlg = resetBtnObj.GetComponent<HorizontalLayoutGroup>();
        rHlg.childAlignment = TextAnchor.MiddleCenter;
        rHlg.childControlWidth = false;
        rHlg.childControlHeight = false;
        rHlg.spacing = 10;
        
        CreateIcon(resetBtnObj.transform, trashSprite, "Icon", 24);
        
        GameObject rTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        rTextObj.transform.SetParent(resetBtnObj.transform, false);
        TextMeshProUGUI rText = rTextObj.GetComponent<TextMeshProUGUI>();
        rText.text = "Reset All";
        rText.fontSize = 20;
        rText.fontStyle = FontStyles.Bold;
        rText.color = redColor;
        rText.alignment = TextAlignmentOptions.Center;
        rText.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);

        paparData.butangReset = resetBtnObj.GetComponent<Button>();
        UnityEngine.Events.UnityAction resetAction = new UnityEngine.Events.UnityAction(paparData.ResetSemuaRekod);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(paparData.butangReset.onClick, resetAction);

        // Table Area
        GameObject tableArea = new GameObject("TableArea", typeof(RectTransform), typeof(Image));
        tableArea.transform.SetParent(contentProgressCSV.transform, false);
        RectTransform tableRect = tableArea.GetComponent<RectTransform>();
        tableRect.anchorMin = new Vector2(0, 0);
        tableRect.anchorMax = new Vector2(1, 1);
        tableRect.offsetMin = new Vector2(0, 0);
        tableRect.offsetMax = new Vector2(0, -70);
        
        Image tableBg = tableArea.GetComponent<Image>();
        tableBg.color = Color.white;
        Outline tableOutline = tableArea.AddComponent<Outline>();
        tableOutline.effectColor = borderBlue;
        tableOutline.effectDistance = new Vector2(2, -2);

        // Table Header
        GameObject tableHeader = new GameObject("TableHeader", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup));
        tableHeader.transform.SetParent(tableArea.transform, false);
        RectTransform thRect = tableHeader.GetComponent<RectTransform>();
        thRect.anchorMin = new Vector2(0, 1);
        thRect.anchorMax = new Vector2(1, 1);
        thRect.pivot = new Vector2(0.5f, 1);
        thRect.anchoredPosition = new Vector2(0, 0);
        thRect.sizeDelta = new Vector2(0, 40);

        Image thImg = tableHeader.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#ECF0F1", out Color thColor);
        thImg.color = thColor;
        
        HorizontalLayoutGroup thHlg = tableHeader.GetComponent<HorizontalLayoutGroup>();
        thHlg.childAlignment = TextAnchor.MiddleLeft;
        thHlg.childForceExpandWidth = false;
        thHlg.childControlWidth = true;
        thHlg.padding = new RectOffset(20, 20, 0, 0);

        string[] headers = { "Date & Time", "Name", "Module", "Activity", "Jumlah Siap", "Status" };
        float[] hWidths = { 260, 150, 150, 160, 100, 110 };

        for (int i = 0; i < headers.Length; i++)
        {
            GameObject hTextObj = new GameObject("H_" + headers[i], typeof(RectTransform), typeof(TextMeshProUGUI));
            hTextObj.transform.SetParent(tableHeader.transform, false);
            TextMeshProUGUI hText = hTextObj.GetComponent<TextMeshProUGUI>();
            hText.text = headers[i];
            hText.fontSize = 18;
            hText.fontStyle = FontStyles.Bold;
            hText.color = darkBlue;
            hText.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Midline;
            hText.GetComponent<RectTransform>().sizeDelta = new Vector2(hWidths[i], 40);

            LayoutElement le = hTextObj.AddComponent<LayoutElement>();
            le.minWidth = hWidths[i];
            le.preferredWidth = hWidths[i];
        }

        // Table Content
        GameObject scrollContainer = new GameObject("ScrollContainer", typeof(RectTransform), typeof(Image), typeof(Mask), typeof(ScrollRect));
        scrollContainer.transform.SetParent(tableArea.transform, false);
        RectTransform scRect = scrollContainer.GetComponent<RectTransform>();
        scRect.anchorMin = new Vector2(0, 0);
        scRect.anchorMax = new Vector2(1, 1);
        scRect.offsetMin = new Vector2(0, 0);
        scRect.offsetMax = new Vector2(0, -40);

        scrollContainer.GetComponent<Image>().color = new Color(1,1,1,0.01f);
        scrollContainer.GetComponent<Mask>().showMaskGraphic = false;

        GameObject contentContainer = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        contentContainer.transform.SetParent(scrollContainer.transform, false);
        RectTransform ccRect = contentContainer.GetComponent<RectTransform>();
        ccRect.anchorMin = new Vector2(0, 1);
        ccRect.anchorMax = new Vector2(1, 1);
        ccRect.pivot = new Vector2(0.5f, 1);
        ccRect.anchoredPosition = new Vector2(0, 0);
        ccRect.sizeDelta = new Vector2(0, 0);

        VerticalLayoutGroup vlg = contentContainer.GetComponent<VerticalLayoutGroup>();
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandHeight = false;
        
        ContentSizeFitter csf = contentContainer.GetComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect sr = scrollContainer.GetComponent<ScrollRect>();
        sr.content = ccRect;
        sr.horizontal = false;
        sr.vertical = true;
        sr.scrollSensitivity = 15;

        paparData.tableContentContainer = contentContainer.transform;

        // Row Prefab Template (hidden)
        GameObject rowTemplate = new GameObject("RowTemplate", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup));
        rowTemplate.transform.SetParent(contentProgressCSV.transform, false);
        rowTemplate.SetActive(false); // hide template
        
        RectTransform rowRect = rowTemplate.GetComponent<RectTransform>();
        rowRect.sizeDelta = new Vector2(0, 60);

        Image rowBg = rowTemplate.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#F8F9FA", out Color rowColor);
        rowBg.color = rowColor;
        
        HorizontalLayoutGroup rowHlg = rowTemplate.GetComponent<HorizontalLayoutGroup>();
        rowHlg.childAlignment = TextAnchor.MiddleLeft;
        rowHlg.childForceExpandWidth = false;
        rowHlg.childControlWidth = true;
        rowHlg.padding = new RectOffset(20, 20, 0, 0);

        // First 5 cells are text (Date, Name, Module, Activity, Jumlah Siap)
        for (int i = 0; i < 5; i++)
        {
            GameObject cellTextObj = new GameObject("Cell_" + i, typeof(RectTransform), typeof(TextMeshProUGUI));
            cellTextObj.transform.SetParent(rowTemplate.transform, false);
            TextMeshProUGUI cellText = cellTextObj.GetComponent<TextMeshProUGUI>();
            cellText.text = "Data";
            cellText.fontSize = 16;
            cellText.color = darkBlue;
            cellText.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Midline;
            cellText.GetComponent<RectTransform>().sizeDelta = new Vector2(hWidths[i], 60);

            LayoutElement le = cellTextObj.AddComponent<LayoutElement>();
            le.minWidth = hWidths[i];
            le.preferredWidth = hWidths[i];
        }

        // Status Badge (Index 5)
        GameObject badgeObj = new GameObject("Badge", typeof(RectTransform), typeof(Image));
        badgeObj.transform.SetParent(rowTemplate.transform, false);
        RectTransform badgeRect = badgeObj.GetComponent<RectTransform>();
        badgeRect.sizeDelta = new Vector2(80, 24);
        
        Image badgeImg = badgeObj.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#F1C40F", out Color yellowColor);
        yellowColor.a = 0.2f;
        badgeImg.color = yellowColor;
        Outline badgeOutline = badgeObj.AddComponent<Outline>();
        ColorUtility.TryParseHtmlString("#F39C12", out Color badgeBorder);
        badgeOutline.effectColor = badgeBorder;
        badgeOutline.effectDistance = new Vector2(1, -1);

        GameObject badgeTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        badgeTextObj.transform.SetParent(badgeObj.transform, false);
        RectTransform btRect = badgeTextObj.GetComponent<RectTransform>();
        btRect.anchorMin = Vector2.zero;
        btRect.anchorMax = Vector2.one;
        btRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI badgeText = badgeTextObj.GetComponent<TextMeshProUGUI>();
        badgeText.text = "Repeated";
        badgeText.fontSize = 14;
        badgeText.fontStyle = FontStyles.Bold;
        badgeText.color = badgeBorder;
        badgeText.alignment = TextAlignmentOptions.Center;
        
        GameObject badgeWrapper = new GameObject("BadgeWrapper", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        badgeWrapper.transform.SetParent(rowTemplate.transform, false);
        badgeWrapper.GetComponent<RectTransform>().sizeDelta = new Vector2(hWidths[5], 60);
        
        LayoutElement badgeLe = badgeWrapper.GetComponent<LayoutElement>();
        badgeLe.minWidth = hWidths[5];
        badgeLe.preferredWidth = hWidths[5];
        
        HorizontalLayoutGroup bwHlg = badgeWrapper.GetComponent<HorizontalLayoutGroup>();
        bwHlg.childAlignment = TextAnchor.MiddleLeft;
        
        badgeObj.transform.SetParent(badgeWrapper.transform, false);
        badgeObj.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 24);

        paparData.rowPrefab = rowTemplate;

        // Assign to TabManager
        tabManager.tabButtons = tabButtons;
        tabManager.tabContents = tabContents;
        tabManager.tabUnderlines = tabUnderlines;
        tabManager.activeTextColor = activeBlue;
        tabManager.inactiveTextColor = tabGray;

        EditorUtility.SetDirty(paparData);
        EditorUtility.SetDirty(tabManager);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Parent Dashboard UI with Tabs & CSV Grid generated successfully!");
    }

    private static void EnsureSpritesImported()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Sprites/DashboardIcons" });
        bool importedAny = false;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
                importedAny = true;
            }
        }
        if (importedAny)
        {
            AssetDatabase.Refresh();
        }
    }

    private static void CreateIcon(Transform parent, Sprite sprite, string name, float size)
    {
        GameObject iconObj = new GameObject(name, typeof(RectTransform), typeof(Image));
        iconObj.transform.SetParent(parent, false);
        RectTransform iconRect = iconObj.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(size, size);
        Image img = iconObj.GetComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = true;
    }

    private static TMP_InputField CreateInputField(Transform parent, string labelText, string valueText, float yPos, Color labelColor, Color borderColor)
    {
        GameObject labelObj = new GameObject("Label_" + labelText, typeof(RectTransform), typeof(TextMeshProUGUI));
        labelObj.transform.SetParent(parent, false);
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 1);
        labelRect.anchorMax = new Vector2(1, 1);
        labelRect.pivot = new Vector2(0.5f, 1);
        labelRect.anchoredPosition = new Vector2(0, yPos);
        labelRect.sizeDelta = new Vector2(0, 30);
        
        TextMeshProUGUI label = labelObj.GetComponent<TextMeshProUGUI>();
        label.text = labelText;
        label.fontSize = 20;
        label.fontStyle = FontStyles.Bold;
        label.color = labelColor;

        GameObject inputObj = new GameObject("InputField_" + labelText, typeof(RectTransform), typeof(Image));
        inputObj.transform.SetParent(parent, false);
        RectTransform inputRect = inputObj.GetComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0, 1);
        inputRect.anchorMax = new Vector2(0.6f, 1);
        inputRect.pivot = new Vector2(0, 1);
        inputRect.anchoredPosition = new Vector2(0, yPos - 35);
        inputRect.sizeDelta = new Vector2(0, 40);

        Image inputBg = inputObj.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#E5E9EC", out Color inputBgColor);
        inputBg.color = inputBgColor;

        Outline inputOutline = inputObj.AddComponent<Outline>();
        inputOutline.effectColor = borderColor;
        inputOutline.effectDistance = new Vector2(2, -2);

        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(inputObj.transform, false);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 0);
        textRect.offsetMax = new Vector2(-10, 0);

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = valueText;
        text.fontSize = 18;
        ColorUtility.TryParseHtmlString("#2C3E50", out Color textColor);
        text.color = textColor;
        text.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Midline;

        TMP_InputField inputFieldComp = inputObj.AddComponent<TMP_InputField>();
        inputFieldComp.textComponent = text;
        inputFieldComp.text = valueText;

        return inputFieldComp;
    }
}
