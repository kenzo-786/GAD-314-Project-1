using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundBlip : MonoBehaviour
{
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        _audio.spatialBlend = 1.0f;
        _audio.minDistance = 2.0f;
        _audio.maxDistance = 50.0f;
        _audio.rolloffMode = AudioRolloffMode.Linear;

        _audio.Play();

        Destroy(gameObject, _audio.clip.length + 0.5f);
    }
}
