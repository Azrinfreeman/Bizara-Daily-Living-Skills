using UnityEngine;
using UnityEngine.UI; // Wajib ada untuk UI dan Button

[RequireComponent(typeof(Button))] // Ini akan tambah komponen Button secara automatik nanti
public class VideoFullScreen : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isFullScreen = false;

    // Pemboleh ubah untuk simpan memori kedudukan dan saiz asal
    private Vector2 saizAsal;
    private Vector2 posisiAsal;
    private Vector2 anchorMinAsal;
    private Vector2 anchorMaxAsal;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // 1. Simpan semua bentuk dan kedudukan asal video ini
        saizAsal = rectTransform.sizeDelta;
        posisiAsal = rectTransform.anchoredPosition;
        anchorMinAsal = rectTransform.anchorMin;
        anchorMaxAsal = rectTransform.anchorMax;

        // 2. Pasang fungsi klik pada Butang secara automatik
        Button butang = GetComponent<Button>();
        butang.onClick.AddListener(TukarSaizSkrin);
    }

    public void TukarSaizSkrin()
    {
        isFullScreen = !isFullScreen; // Tukar status (buka/tutup)

        if (isFullScreen)
        {
            // --- MOD FULL SCREEN ---
            
            // Bawa video ke lapisan paling hadapan supaya tidak ditutup oleh butang lain
            transform.SetAsLastSibling();

            // Kembangkan bucu (anchor) ke hujung skrin
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            
            // Kosongkan jarak tepi (margin) supaya ia penuh
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
        else
        {
            // --- MOD SAIZ ASAL ---
            
            // Kembalikan semua nilai kepada yang asal
            rectTransform.anchorMin = anchorMinAsal;
            rectTransform.anchorMax = anchorMaxAsal;
            rectTransform.anchoredPosition = posisiAsal;
            rectTransform.sizeDelta = saizAsal;
        }
    }
}