using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TukarGambarModul : MonoBehaviour
{
    [Header("Tetapan UI")]
    public RawImage gambarSasaran; 
    public string kunciSimpananPlayerPrefs = "Gambar_Modul1";

    // Pemboleh ubah baharu untuk pengawal aspect ratio
    private AspectRatioFitter aspectRatioFitter;

    void Awake()
    {
        // Cari komponen AspectRatioFitter pada objek gambar
        if (gambarSasaran != null)
        {
            aspectRatioFitter = gambarSasaran.GetComponent<AspectRatioFitter>();
        }
    }

    void Start()
    {
        MuatGambarDariSimpanan();
    }

    public void BukaGaleriDanPilihGambar()
    {
        NativeGallery.GetImageFromGallery((path_gambar) =>
        {
            Debug.Log("Laluan gambar dipilih: " + path_gambar);
            
            if (path_gambar != null)
            {
                Texture2D teksturGambar = NativeGallery.LoadImageAtPath(path_gambar, -1);
                
                if (teksturGambar != null)
                {
                    gambarSasaran.texture = teksturGambar;
                    
                    // --- KOD BAHARU: Kemas kini Aspect Ratio ---
                    KemasKiniAspectRatio(teksturGambar);
                    
                    PlayerPrefs.SetString(kunciSimpananPlayerPrefs, path_gambar);
                    PlayerPrefs.Save();
                }
            }
        }, "Pilih gambar untuk modul ini", "image/*");
    }

    private void MuatGambarDariSimpanan()
    {
        string pathLama = PlayerPrefs.GetString(kunciSimpananPlayerPrefs, "");

        if (pathLama != "" && File.Exists(pathLama))
        {
            Texture2D teksturLama = NativeGallery.LoadImageAtPath(pathLama, -1);
            if (teksturLama != null)
            {
                gambarSasaran.texture = teksturLama;
                
                // --- KOD BAHARU: Kemas kini Aspect Ratio untuk gambar lama ---
                KemasKiniAspectRatio(teksturLama);
            }
        }
    }

    // --- FUNGSI BAHARU UNTUK KIRA SAIZ GAMBAR ---
    private void KemasKiniAspectRatio(Texture2D tekstur)
    {
        if (aspectRatioFitter != null)
        {
            // Formula: Lebar bahagi Tinggi
            float nisbahGambar = (float)tekstur.width / (float)tekstur.height;
            aspectRatioFitter.aspectRatio = nisbahGambar;
        }
    }
}