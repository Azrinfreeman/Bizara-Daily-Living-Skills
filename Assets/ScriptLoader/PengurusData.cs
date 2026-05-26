using UnityEngine;
using System;

public class PengurusData : MonoBehaviour
{
    [Header("Tetapan Data")]
    // Anda boleh tukar nama ini di Inspector untuk setiap Scene (Cth: "MakanMinum", "ToiletTime")
    public string namaModul = "MakanMinum"; 

    public void SimpanRekodModul()
    {
        // Gunakan namaModul yang ditetapkan untuk mencipta kunci yang berbeza
        string kunciJumlah = namaModul + "_JumlahSiap";
        string kunciTarikh = namaModul + "_TarikhAkhir";

        int jumlahSiap = PlayerPrefs.GetInt(kunciJumlah, 0);
        jumlahSiap = jumlahSiap + 1; 
        
        PlayerPrefs.SetInt(kunciJumlah, jumlahSiap);

        string tarikhHariIni = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
        PlayerPrefs.SetString(kunciTarikh, tarikhHariIni);

        PlayerPrefs.Save(); 

        Debug.Log("Data Disimpan untuk " + namaModul + "! Jumlah: " + jumlahSiap + " kali.");
    }
}