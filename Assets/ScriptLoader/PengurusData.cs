using UnityEngine;
using System;

public class PengurusData : MonoBehaviour
{
    [Header("Tetapan Data")]
    public string namaModul = "MakanMinum"; 
    
    public void SimpanRekodModul()
    {
        // 1. Kekalkan fungsi lama untuk jumlah siap
        string kunciJumlah = namaModul + "_JumlahSiap";
        string kunciTarikh = namaModul + "_TarikhAkhir";

        int jumlahSiap = PlayerPrefs.GetInt(kunciJumlah, 0);
        jumlahSiap = jumlahSiap + 1; 
        
        PlayerPrefs.SetInt(kunciJumlah, jumlahSiap);
        string tarikhHariIni = DateTime.Now.ToString("MM/dd/yyyy, hh:mm:ss tt");
        PlayerPrefs.SetString(kunciTarikh, tarikhHariIni);

        PlayerPrefs.Save(); 

        Debug.Log("Data Disimpan untuk " + namaModul + "! Jumlah: " + jumlahSiap + " kali.");
    }
    
    public void PadamSemuaRekod()
    {
        PlayerPrefs.SetInt(namaModul + "_JumlahSiap", 0);
        PlayerPrefs.SetString(namaModul + "_TarikhAkhir", "Belum ada rekod");
        PlayerPrefs.Save();
        Debug.Log("Semua rekod log dipadamkan.");
    }
}