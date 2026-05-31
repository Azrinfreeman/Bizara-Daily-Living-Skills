using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SidebarActiveState : MonoBehaviour
{
    private Transform btnHome;
    private Transform btnEating;
    private Transform btnToilet;
    private Transform btnClean;
    private Transform btnOrganized;
    private Transform btnKunciIbuBapa;

    private GameObject panelKunciIbuBapa;
    private GameObject panelModIbuBapa;

    private void Start()
    {
        Transform menuArea = transform.Find("MenuArea");
        if (menuArea != null)
        {
            btnHome = menuArea.Find("Btn_Home");
            btnEating = menuArea.Find("Btn_Eating & Drinking");
            btnToilet = menuArea.Find("Btn_Toilet Time");
            btnClean = menuArea.Find("Btn_I Am Clean");
            btnOrganized = menuArea.Find("Btn_I Am Organized");
        }

        // Try to find the settings/parent mode button
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allChildren)
        {
            if (t.name == "Btn_KunciIbuBapaMod" || t.name == "Settings" || t.name == "Btn_Settings") 
            {
                btnKunciIbuBapa = t;
                break;
            }
        }

        // If not found inside Sidebar, try Canvas level
        if (btnKunciIbuBapa == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Transform[] allCanvasChildren = canvas.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in allCanvasChildren)
                {
                    if (t.name == "Btn_KunciIbuBapaMod") 
                    {
                        btnKunciIbuBapa = t;
                        break;
                    }
                }
            }
        }

        // Find panels in Canvas
        Canvas mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas != null)
        {
            Transform[] allCanvasChildren = mainCanvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allCanvasChildren)
            {
                if (t.name == "Panel_KunciIbuBapa") panelKunciIbuBapa = t.gameObject;
                if (t.name == "Panel_ModIbuBapa") panelModIbuBapa = t.gameObject;
            }
        }

        UpdateButtonStates();
    }

    private void Update()
    {
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        bool isParentModeActive = (panelKunciIbuBapa != null && panelKunciIbuBapa.activeSelf) || 
                                  (panelModIbuBapa != null && panelModIbuBapa.activeSelf);

        SetButtonState(btnHome, sceneName == "ModulUtama" && !isParentModeActive);
        SetButtonState(btnEating, sceneName == "Modul1" && !isParentModeActive);
        SetButtonState(btnToilet, sceneName == "Modul2" && !isParentModeActive);
        SetButtonState(btnClean, sceneName == "Modul3" && !isParentModeActive);
        SetButtonState(btnOrganized, sceneName == "Modul4" && !isParentModeActive);

        if (btnKunciIbuBapa != null)
        {
            SetButtonState(btnKunciIbuBapa, isParentModeActive);
        }
    }

    private void SetButtonState(Transform btnTransform, bool isActive)
    {
        if (btnTransform == null) return;

        Image bgImage = btnTransform.GetComponent<Image>();
        if (bgImage != null)
        {
            ColorUtility.TryParseHtmlString(isActive ? "#F0F4F8" : "#1C60A6", out Color bgColor);
            bgImage.color = bgColor;
        }

        Transform textTransform = btnTransform.Find("Text");
        if (textTransform == null)
        {
            textTransform = btnTransform.Find("Text (TMP)");
        }
        
        if (textTransform != null)
        {
            TextMeshProUGUI tmp = textTransform.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.fontStyle = isActive ? FontStyles.Bold : FontStyles.Normal;
                ColorUtility.TryParseHtmlString(isActive ? "#1965B2" : "#FFFFFF", out Color txtColor);
                tmp.color = txtColor;
            }
        }
        else
        {
            // Fallback: search for TMP anywhere in children
            TextMeshProUGUI tmp = btnTransform.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.fontStyle = isActive ? FontStyles.Bold : FontStyles.Normal;
                ColorUtility.TryParseHtmlString(isActive ? "#1965B2" : "#FFFFFF", out Color txtColor);
                tmp.color = txtColor;
            }
        }
        
        // Also update the IconImage color if it exists (like the other buttons have icon images next to text)
        Transform iconTransform = btnTransform.Find("IconImage");
        if (iconTransform == null)
        {
            iconTransform = btnTransform.Find("Icon");
        }
        if (iconTransform != null)
        {
            Image iconImg = iconTransform.GetComponent<Image>();
            if (iconImg != null)
            {
                ColorUtility.TryParseHtmlString(isActive ? "#1965B2" : "#FFFFFF", out Color iconColor);
                iconImg.color = iconColor;
            }
        }
    }
}
