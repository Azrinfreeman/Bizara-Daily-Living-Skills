using UnityEngine;
using TMPro; // Untuk teks
using UnityEngine.UI; // Untuk butang

public class KunciIbuBapa : MonoBehaviour
{
    [Header("UI Elemen")]
    public TextMeshProUGUI teksSoalan;
    public TextMeshProUGUI[] teksButangJawapan; // Masukkan 3 teks butang di sini
    
    [Header("Panel Sasaran")]
    public GameObject panelModIbuBapa; // Panel yang akan dibuka jika jawapan betul
    
    private int jawapanBetul;

    // OnEnable berjalan secara automatik setiap kali panel ini dipaparkan (dibuka)
    void OnEnable()
    {
        BinaSoalanRawak();
    }

    public void BinaSoalanRawak()
    {
        // 1. Jana dua nombor rawak antara 1 hingga 10
        int nombor1 = Random.Range(1, 11);
        int nombor2 = Random.Range(1, 11);
        jawapanBetul = nombor1 + nombor2;

        // 2. Tulis soalan di skrin
        teksSoalan.text = "Untuk Ibu Bapa: Berapakah " + nombor1 + " + " + nombor2 + "?";

        // 3. Pilih butang mana (0, 1, atau 2) yang akan memegang jawapan betul
        int indeksBetul = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            if (i == indeksBetul)
            {
                // Letak jawapan betul
                teksButangJawapan[i].text = jawapanBetul.ToString();
            }
            else
            {
                // Letak jawapan salah yang nampak logik (dekat dengan jawapan betul)
                int jawapanSalah = jawapanBetul + Random.Range(-3, 4);
                
                // Pastikan jawapan salah tidak sama dengan jawapan betul, dan bukan negatif
                while (jawapanSalah == jawapanBetul || jawapanSalah <= 0)
                {
                    jawapanSalah = jawapanBetul + Random.Range(-3, 4);
                }
                
                teksButangJawapan[i].text = jawapanSalah.ToString();
            }
        }
    }

    // Fungsi ini akan disambungkan pada ketiga-tiga butang pilihan
    public void SemakJawapan(int indeksButang)
    {
        // Baca nombor pada butang yang ditekan
        string jawapanDitekan = teksButangJawapan[indeksButang].text;

        if (jawapanDitekan == jawapanBetul.ToString())
        {
            // JAWAPAN BETUL: Buka Papan Pemuka Ibu Bapa dan tutup panel kunci ini
            panelModIbuBapa.SetActive(true);
            gameObject.SetActive(false); 
        }
        else
        {
            // JAWAPAN SALAH: Tutup panel kunci serta-merta (elak teka-teki berulang)
            gameObject.SetActive(false);
        }
    }
}