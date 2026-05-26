using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.IO; // WAJIB ADA untuk fungsi menulis fail ke dalam storan peranti

public class PaparData : MonoBehaviour
{
    [Header("Paparan UI")]
    public TextMeshProUGUI teksPapanPemuka; // Komponen teks utama untuk paparan profil & rekod

    [Header("Komponen Input Nama")]
    public TMP_InputField inputNamaPelajar;
    public Button butangSimpanNama;

    // Pemboleh ubah dalaman untuk menyimpan data sementara sebelum dieksport
    private string namaPelajarSemasa = "";
    private int siapMakan, siapToilet, siapModul3, siapModul4;
    private string tarikhMakan, tarikhToilet, tarikhModul3, tarikhModul4;

    void Awake()
    {
        if (butangSimpanNama != null)
        {
            butangSimpanNama.onClick.AddListener(SimpanNamaPelajar);
        }
    }

    void OnEnable()
    {
        KemaskanPaparan();
    }

    public void KemaskanPaparan()
    {
        // 1. Ambil data profil nama pelajar (Disimpan ke pemboleh ubah kelas)
        namaPelajarSemasa = PlayerPrefs.GetString("NamaPelajar", "Belum Tetap");

        // 2. Ambil data REKOD bagi MODUL 1 (Makan & Minum)
        siapMakan = PlayerPrefs.GetInt("MakanMinum_JumlahSiap", 0);
        tarikhMakan = PlayerPrefs.GetString("MakanMinum_TarikhAkhir", "Belum ada rekod");

        // 3. Ambil data REKOD bagi MODUL 2 (Toilet Time)
        siapToilet = PlayerPrefs.GetInt("ToiletTime_JumlahSiap", 0);
        tarikhToilet = PlayerPrefs.GetString("ToiletTime_TarikhAkhir", "Belum ada rekod");

        // 4. Ambil data REKOD bagi MODUL 3 (Saya Bersih)
        siapModul3 = PlayerPrefs.GetInt("Modul3_JumlahSiap", 0);
        tarikhModul3 = PlayerPrefs.GetString("Modul3_TarikhAkhir", "Belum ada rekod");

        // 5. Ambil data REKOD bagi MODUL 4 (Saya Kemas)
        siapModul4 = PlayerPrefs.GetInt("Modul4_JumlahSiap", 0);
        tarikhModul4 = PlayerPrefs.GetString("Modul4_TarikhAkhir", "Belum ada rekod");


        // Setkan teks input nama mengikut data semasa
        if (inputNamaPelajar != null)
        {
            inputNamaPelajar.text = (namaPelajarSemasa == "Belum Tetap") ? "" : namaPelajarSemasa;
        }

        // 6. Gabungkan keseluruhan laporan log ke dalam satu paparan teks yang kemas
        teksPapanPemuka.text = "<b>PROFIL PELAJAR</b>\n" +
                              "Nama Pelajar: " + namaPelajarSemasa + "\n\n" +
                              
                              "<b>REKOD PROGRESS MODUL</b>\n\n" +
                              
                              "<b>Modul 1: Makan & Minum</b>\n" +
                              "Jumlah Bersiap: " + siapMakan + " kali\n" +
                              "Tarikh Akhir: " + tarikhMakan + "\n\n" +
                              
                              "<b>Modul 2: Toilet Time</b>\n" +
                              "Jumlah Bersiap: " + siapToilet + " kali\n" +
                              "Tarikh Akhir: " + tarikhToilet + "\n\n" +
                              
                              "<b>Modul 3: Saya Bersih</b>\n" +
                              "Jumlah Bersiap: " + siapModul3 + " kali\n" +
                              "Tarikh Akhir: " + tarikhModul3 + "\n\n" +
                              
                              "<b>Modul 4: Saya Kemas</b>\n" +
                              "Jumlah Bersiap: " + siapModul4 + " kali\n" +
                              "Tarikh Akhir: " + tarikhModul4;
    }

    public void SimpanNamaPelajar()
    {
        if (inputNamaPelajar != null && !string.IsNullOrEmpty(inputNamaPelajar.text))
        {
            PlayerPrefs.SetString("NamaPelajar", inputNamaPelajar.text);
            PlayerPrefs.Save();
            
            Debug.Log("Nama pelajar berjaya disimpan: " + inputNamaPelajar.text);

            KemaskanPaparan();
        }
    }

    // --- FUNGSI BAHARU: EKSPORT DATA KE EXCEL (CSV) ---
    public void EksportKeExcel()
    {
        // Pastikan data dipaparkan semula dengan nilai paling segar sebelum mula eksport
        KemaskanPaparan();

        // 1. Tetapkan nama fail yang unik mengikut nama pelajar
        string namaFail = "Laporan_Progress_" + namaPelajarSemasa.Replace(" ", "_") + ".csv";
        string laluanPenuh = Path.Combine(Application.persistentDataPath, namaFail);

        // 2. Bina struktur teks berformat CSV (Sesuai untuk Microsoft Excel & Google Sheets)
        // Kita gunakan pembahagi semikolon (;) untuk mengelakkan ralat susunan petak pada komputer Windows
        string isiKandungan = "sep=;\n"; 
        isiKandungan += "PROFIL PELAJAR;\n";
        isiKandungan += "Nama Pelajar: ;" + namaPelajarSemasa + ";\n\n";
        
        isiKandungan += "LAPORAN KEMAJUAN MODUL AUTISME;\n";
        isiKandungan += "Nama Modul;Jumlah Disiapkan (Kali);Tarikh & Masa Akhir Disiapkan;\n";
        isiKandungan += "Modul 1: Makan & Minum;" + siapMakan + ";" + tarikhMakan + ";\n";
        isiKandungan += "Modul 2: Toilet Time;" + siapToilet + ";" + tarikhToilet + ";\n";
        isiKandungan += "Modul 3: Saya Bersih;" + siapModul3 + ";" + tarikhModul3 + ";\n";
        isiKandungan += "Modul 4: Saya Kemas;" + siapModul4 + ";" + tarikhModul4 + ";\n";

        // 3. Simpan data ke dalam memori storan peranti
        try
        {
            File.WriteAllText(laluanPenuh, isiKandungan, System.Text.Encoding.UTF8);
            Debug.Log("Sistem: Fail Excel (CSV) berjaya dihasilkan di: " + laluanPenuh);
            
            // Mengaktifkan bukaan tetingkap folder automatik jika anda sedang menguji di dalam Unity Editor (PC)
            #if UNITY_EDITOR
            Application.OpenURL(Application.persistentDataPath); 
            #endif
        }
        catch (System.Exception e)
        {
            Debug.LogError("Sistem: Gagal mengeksport fail Excel. Ralat: " + e.Message);
        }
    }
}