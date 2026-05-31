using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButangTukarBahasa : MonoBehaviour
{
    private Button butang;
    public TextMeshProUGUI teksLabel;

    void Awake()
    {
        butang = GetComponent<Button>();
        butang.onClick.AddListener(TukarBahasa);
        if (teksLabel == null) teksLabel = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        PengurusBahasa.OnBahasaDitukar += KemaskiniPaparan;
        KemaskiniPaparan();
    }

    void OnDisable()
    {
        PengurusBahasa.OnBahasaDitukar -= KemaskiniPaparan;
    }

    private void TukarBahasa()
    {
        if (PengurusBahasa.Instance != null)
        {
            PengurusBahasa.Instance.TukarBahasa();
        }
    }

    private void KemaskiniPaparan()
    {
        if (teksLabel != null && PengurusBahasa.Instance != null)
        {
            string bahasa = PengurusBahasa.Instance.BahasaSemasa;
            teksLabel.text = (bahasa == "EN") ? "English" : "Melayu";
        }
    }
}
