using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        AudioListener.volume = value; // Simplified; use AudioMixer for full control
    }

    public void SetSFXVolume(float value)
    {
        // You can assign to specific mixers later
        Debug.Log("SFX Volume: " + value);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}
