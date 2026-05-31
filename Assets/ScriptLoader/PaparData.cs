using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.IO; 
using System.Collections.Generic;

public class PaparData : MonoBehaviour
{
    [Header("Paparan UI")]
    public TextMeshProUGUI teksPapanPemuka; // Legacy

    [Header("Komponen Input Profil")]
    public TMP_InputField inputNamaPelajar;
    public TMP_InputField inputUmurPelajar;
    public Button butangSimpan;
    public Image[] avatarImages;
    public Outline[] avatarOutlines; 

    [Header("Komponen Progress & CSV")]
    public Transform tableContentContainer;
    public GameObject rowPrefab;
    public Button butangReset;
    public Button butangDownload;
    public Sprite starSprite;

    private string namaPelajarSemasa = "";
    private string umurPelajarSemasa = "";
    private int avatarSemasa = 0;
    private int avatarDipilihSementara = 0;

    void Awake()
    {
        if (butangSimpan != null)
        {
            butangSimpan.onClick.AddListener(SimpanDataProfil);
        }
        if (butangDownload != null)
        {
            butangDownload.onClick.AddListener(EksportKeExcel);
        }
        if (butangReset != null)
        {
            butangReset.onClick.AddListener(ResetSemuaRekod);
        }
    }

    void OnEnable()
    {
        KemaskanPaparan();
        BinaJadualRekod();
    }

    public void PilihAvatar(int index)
    {
        avatarDipilihSementara = index;
        KemaskiniVisualAvatar(avatarDipilihSementara);
    }

    private void KemaskiniVisualAvatar(int selectedIndex)
    {
        if (avatarOutlines != null && avatarOutlines.Length > 0)
        {
            for (int i = 0; i < avatarOutlines.Length; i++)
            {
                if (avatarOutlines[i] != null)
                {
                    avatarOutlines[i].enabled = (i == selectedIndex);
                }
            }
        }
    }

    public void KemaskanPaparan()
    {
        namaPelajarSemasa = PlayerPrefs.GetString("NamaPelajar", "Belum Tetap");
        umurPelajarSemasa = PlayerPrefs.GetString("UmurPelajar", "");
        avatarSemasa = PlayerPrefs.GetInt("AvatarPelajar", 0);
        avatarDipilihSementara = avatarSemasa;

        if (inputNamaPelajar != null)
        {
            inputNamaPelajar.text = (namaPelajarSemasa == "Belum Tetap") ? "" : namaPelajarSemasa;
        }
        
        if (inputUmurPelajar != null)
        {
            inputUmurPelajar.text = umurPelajarSemasa;
        }

        KemaskiniVisualAvatar(avatarSemasa);
    }

    public void BinaJadualRekod()
    {
        if (tableContentContainer == null || rowPrefab == null) return;

        // Kosongkan jadual sedia ada
        foreach (Transform child in tableContentContainer)
        {
            Destroy(child.gameObject);
        }

        string namaPelajar = PlayerPrefs.GetString("NamaPelajar", "Belum Tetap");
        
        string[] modulKeys = { "MakanMinum", "ToiletTime", "Modul3", "Modul4" };
        string[] modulNames = { "Makan & Minum", "Toilet Time", "Saya Bersih", "Saya Kemas" };

        for (int i = 0; i < 4; i++)
        {
            int jumlahSiap = PlayerPrefs.GetInt(modulKeys[i] + "_JumlahSiap", 0);
            string tarikhAkhir = PlayerPrefs.GetString(modulKeys[i] + "_TarikhAkhir", "Belum ada rekod");

            GameObject row = Instantiate(rowPrefab, tableContentContainer);
            row.SetActive(true);
            
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 5)
            {
                texts[0].text = tarikhAkhir;
                texts[1].text = namaPelajar;
                texts[2].text = modulNames[i];
                texts[3].text = "Semua Aktiviti"; // Placeholder aktiviti
                texts[4].text = jumlahSiap.ToString();
            }

            // Status Badge
            Transform badgeTransform = row.transform.Find("BadgeWrapper/Badge");
            if (badgeTransform != null)
            {
                Image badgeImg = badgeTransform.GetComponent<Image>();
                UnityEngine.UI.Outline badgeOutline = badgeTransform.GetComponent<UnityEngine.UI.Outline>();
                Transform textTransform = badgeTransform.Find("Text");
                
                if (textTransform != null && badgeImg != null && badgeOutline != null)
                {
                    TextMeshProUGUI badgeText = textTransform.GetComponent<TextMeshProUGUI>();
                    Color textColor = Color.black;
                    Color bgColor = Color.white;

                    if (jumlahSiap > 1) 
                    {
                        badgeText.text = "Repeated";
                        ColorUtility.TryParseHtmlString("#2980B9", out textColor); // Dark Blue
                    }
                    else if (jumlahSiap == 1) 
                    {
                        badgeText.text = "Completed";
                        ColorUtility.TryParseHtmlString("#27AE60", out textColor); // Dark Green
                    }
                    else 
                    {
                        badgeText.text = "Not Started";
                        ColorUtility.TryParseHtmlString("#7F8C8D", out textColor); // Dark Gray
                    }

                    bgColor = textColor; 
                    bgColor.a = 0.2f;

                    badgeImg.color = bgColor;
                    badgeOutline.effectColor = textColor;
                    badgeText.color = textColor;
                }
            }
        }
    }

    public void SimpanDataProfil()
    {
        if (inputNamaPelajar != null && !string.IsNullOrEmpty(inputNamaPelajar.text))
        {
            PlayerPrefs.SetString("NamaPelajar", inputNamaPelajar.text);
        }
        
        if (inputUmurPelajar != null)
        {
            PlayerPrefs.SetString("UmurPelajar", inputUmurPelajar.text);
        }

        PlayerPrefs.SetInt("AvatarPelajar", avatarDipilihSementara);
        PlayerPrefs.Save();
        
        Debug.Log("Profil pelajar berjaya disimpan.");
        KemaskanPaparan();

        // Update the sidebar immediately
        ProfilSidebar sidebar = Object.FindFirstObjectByType<ProfilSidebar>();
        if (sidebar != null)
        {
            sidebar.KemaskiniProfil();
        }
    }

    public void ResetSemuaRekod()
    {
        string[] modulKeys = { "MakanMinum", "ToiletTime", "Modul3", "Modul4" };
        foreach(string key in modulKeys)
        {
            PlayerPrefs.SetInt(key + "_JumlahSiap", 0);
            PlayerPrefs.SetString(key + "_TarikhAkhir", "Belum ada rekod");
        }
        PlayerPrefs.Save();
        BinaJadualRekod();
        Debug.Log("Sistem: Semua rekod dipadamkan dari Dashboard.");
    }

    public void EksportKeExcel()
    {
        KemaskanPaparan();

        string namaFail = "Laporan_Progress_" + namaPelajarSemasa.Replace(" ", "_") + ".csv";
        string laluanPenuh = Path.Combine(Application.persistentDataPath, namaFail);

        string isiKandungan = "sep=;\n"; 
        isiKandungan += "PROFIL PELAJAR;\n";
        isiKandungan += "Nama Pelajar: ;" + namaPelajarSemasa + ";\n";
        isiKandungan += "Umur: ;" + umurPelajarSemasa + ";\n\n";
        
        isiKandungan += "LAPORAN KEMAJUAN TERPERINCI;\n";
        isiKandungan += "Tarikh & Masa;Nama Pelajar;Modul;Aktiviti;Jumlah Siap;Status;\n";

        string[] modulKeys = { "MakanMinum", "ToiletTime", "Modul3", "Modul4" };
        string[] modulNames = { "Makan & Minum", "Toilet Time", "Saya Bersih", "Saya Kemas" };

        for (int i = 0; i < 4; i++)
        {
            int jumlahSiap = PlayerPrefs.GetInt(modulKeys[i] + "_JumlahSiap", 0);
            string tarikhAkhir = PlayerPrefs.GetString(modulKeys[i] + "_TarikhAkhir", "Belum ada rekod");
            string status = jumlahSiap > 1 ? "Repeated" : (jumlahSiap == 1 ? "Completed" : "Not Started");

            isiKandungan += tarikhAkhir + ";" + namaPelajarSemasa + ";" + modulNames[i] + ";Semua Aktiviti;" + jumlahSiap + ";" + status + ";\n";
        }

        try
        {
            File.WriteAllText(laluanPenuh, isiKandungan, System.Text.Encoding.UTF8);
            Debug.Log("Sistem: Fail Excel (CSV) berjaya dihasilkan di: " + laluanPenuh);
            
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