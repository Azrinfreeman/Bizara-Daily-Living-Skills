using UnityEngine;
using TMPro; // Diperlukan untuk mengawal TextMeshPro

public class KemaskiniNamaTeks : MonoBehaviour
{
    private TextMeshProUGUI teksTMP;

    [Header("Tetapan Ayat")]
    [Tooltip("Tulis ayat anda di sini. Gunakan penanda {nama} di tempat yang anda mahu nama pelajar muncul.")]
    public string formatAyat = "{nama} pegang sudu.";

    void Awake()
    {
        // Ambil komponen TextMeshPro yang ada pada objek ini
        teksTMP = GetComponent<TextMeshProUGUI>();
    }

    // OnEnable berjalan setiap kali panel/teks ini diaktifkan (SetActive = true)
    void OnEnable()
    {
        KemasKiniTeksPelajar();
    }

    public void KemasKiniTeksPelajar()
    {
        if (teksTMP == null) return;

        // 1. Ambil nama dari PlayerPrefs. 
        // Jika ibu bapa belum tetapkan nama lagi, ia akan guna "Saya" sebagai sandaran (fallback)
        string namaPelajar = PlayerPrefs.GetString("NamaPelajar", "Saya");

        // 2. Gantikan penanda {nama} dengan nama sebenar pelajar
        string ayatBaharu = formatAyat.Replace("{nama}", namaPelajar);

        // 3. Masukkan ayat baharu ke dalam TextMeshPro
        teksTMP.text = ayatBaharu;
    }
}