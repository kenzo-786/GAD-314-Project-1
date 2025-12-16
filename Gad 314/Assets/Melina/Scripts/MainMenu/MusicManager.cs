using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return;
        StartCoroutine(FadeOutIn(newClip));
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = t / fadeDuration;
            yield return null;
        }
        audioSource.volume = 1f;
    }

    IEnumerator FadeOutIn(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }
    }
}