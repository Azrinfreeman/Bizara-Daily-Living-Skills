using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TeksTerjemahan : MonoBehaviour
{
    public string kunciTerjemahan;
    private TextMeshProUGUI teksUI;

    void Awake()
    {
        teksUI = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        PengurusBahasa.OnBahasaDitukar += KemaskiniTeks;
        KemaskiniTeks(); // Update immediately when enabled
    }

    void OnDisable()
    {
        PengurusBahasa.OnBahasaDitukar -= KemaskiniTeks;
    }

    public void KemaskiniTeks()
    {
        if (PengurusBahasa.Instance != null && !string.IsNullOrEmpty(kunciTerjemahan))
        {
            if (teksUI == null) teksUI = GetComponent<TextMeshProUGUI>();
            teksUI.text = PengurusBahasa.Instance.Terjemah(kunciTerjemahan);
        }
    }
}
