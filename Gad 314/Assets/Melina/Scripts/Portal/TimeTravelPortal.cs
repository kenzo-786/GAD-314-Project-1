using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class TimeTravelPortal : MonoBehaviour
{
    public string sceneToLoad;
    public float effectDuration = 1.2f;
    public float fadeDuration = 0.6f;

    [Header("Particles")]
    public ParticleSystem burstParticles;

    [Header("Post Processing")]
    public Volume timeTravelVolume;
    LensDistortion lensDist;
    ChromaticAberration chroma;
    Vignette vign;

    FadeController fadeController;
    bool isTeleporting = false;

    void Start()
    {
        fadeController = FindFirstObjectByType<FadeController>();

        timeTravelVolume.profile.TryGet(out lensDist);
        timeTravelVolume.profile.TryGet(out chroma);
        timeTravelVolume.profile.TryGet(out vign);

        lensDist.intensity.Override(0);
        chroma.intensity.Override(0);
        vign.intensity.Override(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DoTeleport());
    }

    IEnumerator DoTeleport()
    {
        if (isTeleporting) yield break;
        isTeleporting = true;

        if (burstParticles) burstParticles.Play();

        float t = 0;
        while (t < effectDuration)
        {
            t += Time.deltaTime;
            float p = t / effectDuration;

            lensDist.intensity.Override(Mathf.Lerp(0, -0.8f, p));
            chroma.intensity.Override(Mathf.Lerp(0, 1f, p));
            vign.intensity.Override(Mathf.Lerp(0, 0.4f, p));

            yield return null;
        }

        yield return fadeController.StartCoroutine(fadeController.FadeOut(fadeDuration));

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!load.isDone) yield return null;

        yield return fadeController.StartCoroutine(fadeController.FadeIn(fadeDuration));
    }
}