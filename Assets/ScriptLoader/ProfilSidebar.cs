using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilSidebar : MonoBehaviour
{
    public Image avatarImage;
    public TextMeshProUGUI namaText;
    public Sprite[] avatarSprites;

    void OnEnable()
    {
        KemaskiniProfil();
    }

    public void KemaskiniProfil()
    {
        string nama = PlayerPrefs.GetString("NamaPelajar", "Pemain");
        int avatarIndex = PlayerPrefs.GetInt("AvatarPelajar", 0);

        if (namaText != null)
        {
            namaText.text = nama;
        }

        if (avatarImage != null && avatarSprites != null && avatarIndex >= 0 && avatarIndex < avatarSprites.Length)
        {
            avatarImage.sprite = avatarSprites[avatarIndex];
        }
    }
}
