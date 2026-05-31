using UnityEngine;
using UnityEngine.Video;
using System.IO;

[RequireComponent(typeof(VideoPlayer))]
public class MuatVideoModul : MonoBehaviour
{
    [Header("Tetapan Video")]
    [Tooltip("Kunci yang digunakan dalam PlayerPrefs untuk mencari video. Contoh: MakanMinum_CustomVideo")]
    public string kunciSimpananPlayerPrefs = "MakanMinum_CustomVideo";

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        MuatVideoDariSimpanan();
    }

    public void MuatVideoDariSimpanan()
    {
        string pathLama = PlayerPrefs.GetString(kunciSimpananPlayerPrefs, "");

        if (!string.IsNullOrEmpty(pathLama) && File.Exists(pathLama))
        {
            // Tukar URL video kepada video dari peranti (Local Storage)
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = pathLama;
            
            Debug.Log("Video peribadi dimuatkan dari: " + pathLama);
        }
        else
        {
            Debug.Log("Tiada video peribadi dijumpai untuk kunci: " + kunciSimpananPlayerPrefs + ". Menggunakan video lalai (default).");
        }
    }
}
