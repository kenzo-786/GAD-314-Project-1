using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class IntroSequence : MonoBehaviour
{
    [Header("Configuration")]
    public string nextSceneName = "MainMenu";

    public bool allowSkip = true;

    [Header("Timing")]
    public float fadeDuration = 1.0f;
    public float displayDuration = 2.0f;

    [Header("Logos")]
    [Tooltip("Drag your logo sprites here in order.")]
    public Sprite[] logoSequence;

    [Header("UI References")]
    public Image logoDisplay;
    public CanvasGroup fadeGroup;

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private void Update()
    {
        if (allowSkip && Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            LoadNextScene();
        }
    }

    private IEnumerator PlayIntro()
    {
        fadeGroup.alpha = 0f;

        foreach (Sprite logo in logoSequence)
        {
            logoDisplay.sprite = logo;

            logoDisplay.preserveAspect = true;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeGroup.alpha = timer / fadeDuration;
                yield return null;
            }
            fadeGroup.alpha = 1f;

            yield return new WaitForSeconds(displayDuration);

            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeGroup.alpha = 1f - (timer / fadeDuration);
                yield return null;
            }
            fadeGroup.alpha = 0f;

            yield return new WaitForSeconds(0.5f);
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

}
