using UnityEngine;
using UnityEngine.UI;
using TMPro; // In case we want to update a text label

public class BGMVolumeController : MonoBehaviour
{
    [Tooltip("The slider that controls the volume.")]
    public Slider volumeSlider;
    
    [Tooltip("If assigned, will only change this AudioSource. Otherwise, changes global AudioListener.volume.")]
    public AudioSource targetBGM;
    
    private const string BGM_VOLUME_KEY = "BGM_Volume_v2";

    void Start()
    {
        if (volumeSlider == null)
        {
            volumeSlider = GetComponent<Slider>();
        }
        
        // Load saved volume (default is 0.4f)
        float savedVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.4f);
        
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        
        // Apply initial volume
        ApplyVolume(savedVolume);
    }

    public void OnVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }
    
    private void ApplyVolume(float value)
    {
        if (targetBGM != null)
        {
            targetBGM.volume = value;
        }
        else
        {
            // Global volume control
            AudioListener.volume = value;
        }
    }
}
