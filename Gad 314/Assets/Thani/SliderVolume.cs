using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI ValueText;
    [SerializeField] private AudioMixMode MixMode;

    public void OnChangeSlider(float Value)
    {
        ValueText.SetText($"{Value.ToString("N4")}");

        switch(MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                audioSource.volume = Value;
                break;
            case AudioMixMode.LinearMixerVolume:
                mixer.SetFloat("Volume", (-80 + Value * 100));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }
    }

    public enum AudioMixMode
    {
        LinearAudioSourceVolume, LinearMixerVolume, LogrithmicMixerVolume
    }
}
