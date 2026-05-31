using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PengurusMediaDashboard : MonoBehaviour
{
    [Header("UI Components")]
    public Dropdown modulDropdown; // Changed to standard Dropdown
    public RawImage previewImage;
    public TextMeshProUGUI dashboardStatusText;

    private AspectRatioFitter aspectRatioFitter;

    // The keys mapping to Dropdown index 0-7
    private string[] imageKeys = {
        "MakanMinum_CustomImage",
        "ToiletTime_CustomImage",
        "SayaBersih_CustomImage",
        "SayaKemas_CustomImage",
        "Gambar_Modul1",
        "Gambar_Modul2",
        "Gambar_Modul3",
        "Gambar_Modul4"
    };

    private string[] videoKeys = {
        "MakanMinum_CustomVideo",
        "ToiletTime_CustomVideo",
        "SayaBersih_CustomVideo",
        "SayaKemas_CustomVideo",
        "Video_Modul1",
        "Video_Modul2",
        "Video_Modul3",
        "Video_Modul4"
    };

    void Awake()
    {
        if (previewImage != null)
        {
            aspectRatioFitter = previewImage.GetComponent<AspectRatioFitter>();
        }

        if (modulDropdown != null)
        {
            modulDropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
    }

    void OnEnable()
    {
        SemakStatusDashboard();
        if (modulDropdown != null)
        {
            OnDropdownChanged(modulDropdown.value);
        }
    }

    // --- Dashboard Upload (Left Panel) ---
    public void MuatNaikVideoDashboard()
    {
        NativeGallery.GetVideoFromGallery((path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                PlayerPrefs.SetString("Dashboard_CustomVideo", path);
                PlayerPrefs.Save();
                SemakStatusDashboard();
                Debug.Log("Dashboard video saved: " + path);
            }
        }, "Pilih Video untuk Dashboard", "video/*");
    }

    private void SemakStatusDashboard()
    {
        if (dashboardStatusText == null) return;
        
        string dashVideo = PlayerPrefs.GetString("Dashboard_CustomVideo", "");
        if (!string.IsNullOrEmpty(dashVideo) && File.Exists(dashVideo))
        {
            dashboardStatusText.text = "Custom instruction video active.";
            ColorUtility.TryParseHtmlString("#27AE60", out Color c);
            dashboardStatusText.color = c;
        }
        else
        {
            dashboardStatusText.text = "Default instruction video active.";
            ColorUtility.TryParseHtmlString("#7F8C8D", out Color c2);
            dashboardStatusText.color = c2;
        }
    }

    // --- Module Uploads (Right Panel) ---
    public void MuatNaikGambarModul()
    {
        int currentIndex = modulDropdown != null ? modulDropdown.value : 0;
        string key = imageKeys[currentIndex];

        NativeGallery.GetImageFromGallery((path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                PlayerPrefs.SetString(key, path);
                PlayerPrefs.Save();
                KemaskiniPreview();
                Debug.Log(key + " image saved: " + path);
            }
        }, "Pilih Gambar", "image/*");
    }

    public void MuatNaikVideoModul()
    {
        int currentIndex = modulDropdown != null ? modulDropdown.value : 0;
        string key = videoKeys[currentIndex];

        NativeGallery.GetVideoFromGallery((path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                PlayerPrefs.SetString(key, path);
                PlayerPrefs.Save();
                KemaskiniPreview();
                Debug.Log(key + " video saved: " + path);
            }
        }, "Pilih Video", "video/*");
    }

    private void OnDropdownChanged(int index)
    {
        KemaskiniPreview();
    }

    private void KemaskiniPreview()
    {
        if (previewImage == null || modulDropdown == null) return;

        int currentIndex = modulDropdown.value;
        string imgKey = imageKeys[currentIndex];
        string vidKey = videoKeys[currentIndex];

        string customImage = PlayerPrefs.GetString(imgKey, "");
        string customVideo = PlayerPrefs.GetString(vidKey, "");

        // If custom image exists, show it.
        // If custom video exists, we could try to generate a thumbnail.
        // We'll prioritize Image if both exist, or prioritize whatever was uploaded last? 
        // Let's just show the Image if it exists.
        
        if (!string.IsNullOrEmpty(customImage) && File.Exists(customImage))
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(customImage, -1);
            if (texture != null)
            {
                previewImage.texture = texture;
                previewImage.color = Color.white;
                KemasKiniAspectRatio(texture);
                return;
            }
        }
        
        // If we reach here, no valid image. Can we get video thumbnail?
        if (!string.IsNullOrEmpty(customVideo) && File.Exists(customVideo))
        {
            Texture2D thumbnail = NativeGallery.GetVideoThumbnail(customVideo);
            if (thumbnail != null)
            {
                previewImage.texture = thumbnail;
                previewImage.color = Color.white;
                KemasKiniAspectRatio(thumbnail);
                return;
            }
        }

        // Default placeholder
        previewImage.texture = null;
        previewImage.color = new Color(0.9f, 0.9f, 0.9f, 1f); // Gray placeholder
    }

    private void KemasKiniAspectRatio(Texture2D tekstur)
    {
        if (aspectRatioFitter != null && tekstur != null)
        {
            float nisbahGambar = (float)tekstur.width / (float)tekstur.height;
            aspectRatioFitter.aspectRatio = nisbahGambar;
        }
    }
}
