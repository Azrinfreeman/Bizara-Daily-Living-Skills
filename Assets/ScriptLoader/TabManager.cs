using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabContents;
    public Image[] tabUnderlines;
    
    public Color activeTextColor;
    public Color inactiveTextColor;

    void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }

        // Initialize first tab
        if (tabButtons.Length > 0)
        {
            SwitchTab(0);
        }
    }

    public void SwitchTab(int tabIndex)
    {
        for (int i = 0; i < tabContents.Length; i++)
        {
            if (tabContents[i] != null)
            {
                tabContents[i].SetActive(i == tabIndex);
            }

            if (tabButtons[i] != null)
            {
                var textComponent = tabButtons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.color = (i == tabIndex) ? activeTextColor : inactiveTextColor;
                    textComponent.fontStyle = (i == tabIndex) ? TMPro.FontStyles.Bold : TMPro.FontStyles.Normal;
                }
            }

            if (tabUnderlines != null && i < tabUnderlines.Length && tabUnderlines[i] != null)
            {
                tabUnderlines[i].enabled = (i == tabIndex);
            }
        }
    }
}
