using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // --- KOD BAHARU: Wajib ada untuk tukar Scene ---

public class ModulManager : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panelGameTap;
    public GameObject panelGameDrag;
    public GameObject panelGameSequence;
    public GameObject panelReward;

    [Header("Kad Sequence")]
    [Header("Sistem Urutan Kad")]
    public KadSusun[] senaraiKadModul; // Boleh letak 3, 4, atau lebih kad di Inspector

    [Header("Tetapan Masa Delay (Saat)")]
    public float masaDelayTap = 1.0f;
    public float masaDelaySequence = 2.0f;

    public void SelesaiGameTap()
    {
        StartCoroutine(TungguDanTukarPanelTap());
    }

    private IEnumerator TungguDanTukarPanelTap()
    {
        yield return new WaitForSeconds(masaDelayTap);
        panelGameTap.SetActive(false);
        panelGameDrag.SetActive(true);
    }

    public void SelesaiGameDrag()
    {
        StartCoroutine(TungguDanTukarPanelDrag());
    }

    private IEnumerator TungguDanTukarPanelDrag()
    {
        yield return new WaitForSeconds(1.5f);
        panelGameDrag.SetActive(false);
        RawakkanKad();
        panelGameSequence.SetActive(true);
    }

    private void RawakkanKad()
    {
        if (senaraiKadModul == null || senaraiKadModul.Length == 0)
            return;

        int jumlahKad = senaraiKadModul.Length;
        Vector3[] senaraiPosisi = new Vector3[jumlahKad];

        // 1. Ambil semua posisi asal kad semasa di skrin
        for (int i = 0; i < jumlahKad; i++)
        {
            senaraiPosisi[i] = senaraiKadModul[i].transform.position;
        }

        bool susunanMasihBetul = true;

        // Loop ini akan terus menggaul selagi susunan kebetulan menjadi betul (1, 2, 3, 4)
        while (susunanMasihBetul)
        {
            // Gaul (Shuffle) senarai posisi
            for (int i = 0; i < jumlahKad; i++)
            {
                Vector3 simpanSementara = senaraiPosisi[i];
                int indeksRawak = UnityEngine.Random.Range(i, jumlahKad);
                senaraiPosisi[i] = senaraiPosisi[indeksRawak];
                senaraiPosisi[indeksRawak] = simpanSementara;
            }

            // Semak: Adakah koordinat X setiap kad tersusun dari kiri ke kanan mengikut urutan indeks?
            bool adaYangSalah = false;
            for (int i = 0; i < jumlahKad - 1; i++)
            {
                // Jika kedudukan X kad sebelumnya lebih besar dari kad selepasnya, bermakna ia sudah berselerak
                if (senaraiPosisi[i].x > senaraiPosisi[i + 1].x)
                {
                    adaYangSalah = true;
                    break;
                }
            }

            // Jika ada yang salah (berselerak), kita boleh keluar dari loop
            if (adaYangSalah)
            {
                susunanMasihBetul = false;
            }
        }

        // 2. Berikan kedudukan baharu yang telah berselerak kepada kad-kad tersebut
        for (int i = 0; i < jumlahKad; i++)
        {
            senaraiKadModul[i].transform.position = senaraiPosisi[i];
            senaraiKadModul[i].posisiAsal = senaraiPosisi[i]; // Kemas kini posisi asal untuk sistem drag drop
        }
    }

    // Tambahkan fungsi ini di bahagian bawah skrip ModulManager.cs
    public void SemakSusunanKad()
    {
        if (senaraiKadModul == null || senaraiKadModul.Length == 0)
            return;

        bool semuaBetul = true;

        // 1. Menyemak adakah setiap kad berada dalam kedudukan X dari kiri ke kanan
        for (int i = 0; i < senaraiKadModul.Length - 1; i++)
        {
            if (
                senaraiKadModul[i].transform.position.x
                > senaraiKadModul[i + 1].transform.position.x
            )
            {
                semuaBetul = false; // Ada kad yang salah susunan
                break;
            }
        }

        // 2. Jika susunan semua kad sudah betul (1-2-3-4)
        if (semuaBetul)
        {
            Debug.Log("Sistem: Tahniah! Susunan adalah BETUL!");

            // Loop untuk kunci kesemua kad secara automatik, tidak kira 3 atau 4 kad
            for (int i = 0; i < senaraiKadModul.Length; i++)
            {
                senaraiKadModul[i].sudahDikunci = true;
            }

            // Jalankan animasi skrin ganjaran
            StartCoroutine(TukarKeReward());
        }
        else
        {
            Debug.Log("Sistem: Susunan masih ada yang salah. Cuba lagi.");
        }
    }

    private IEnumerator TukarKeReward()
    {
        yield return new WaitForSeconds(masaDelaySequence);
        panelGameSequence.SetActive(false);
        panelReward.SetActive(true);

        // Cari komponen PengurusData yang ada pada GameManager
        PengurusData pengurusData = GetComponent<PengurusData>();
        if (pengurusData != null)
        {
            // DIBETULKAN: Tukar dari SimpanRekodMakanMinum() kepada SimpanRekodModul()
            pengurusData.SimpanRekodModul();
        }
    }

    // --- FUNGSI BAHARU UNTUK SKRIN GANJARAN ---

    public void UlangModul()
    {
        Debug.Log("Ulang Semula Modul...");
        // Ini akan "reload" scene semasa, mengembalikan semua panel dan susunan seperti mula-mula
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KembaliKeMenuUtama()
    {
        Debug.Log("Kembali ke Modul Utama...");
        // NOTA PENTING: Tukar "ModulUtama" di bawah ini mengikut nama fail Scene menu utama anda!
        SceneManager.LoadScene("ModulUtama");
    }
}
