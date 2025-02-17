using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign your Audio Mixer in the Inspector
    public Slider masterVolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
        }
        
    }

    public void SetMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    private void LoadVolume()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        SetMasterVolume();
    }
}