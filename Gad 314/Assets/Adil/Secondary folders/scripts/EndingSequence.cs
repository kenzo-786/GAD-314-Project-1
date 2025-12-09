using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingSequence : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;

    [Header("Timing Settings")]
    public float initialBlackScreen = 1f;
    public float textFadeDuration = 2.0f;
    public float waitBetweenTexts = 1.0f;
    public float lingerDuration = 4.0f;

    [Header("Next Scene")]
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        SetAlpha(topText, 0f);
        SetAlpha(bottomText, 0f);

        StartCoroutine(PlayEndingSequence());
    }

    private IEnumerator PlayEndingSequence()
    {
        yield return new WaitForSeconds(initialBlackScreen);

        yield return StartCoroutine(FadeTextIn(topText));

        yield return new WaitForSeconds(waitBetweenTexts);

        yield return StartCoroutine(FadeTextIn(bottomText));

        yield return new WaitForSeconds(lingerDuration);

        Debug.Log("Ending sequence finished. Loading Main Menu.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        if (text == null) yield break;

        float timer = 0f;
        while (timer < textFadeDuration)
        {
            timer += Time.deltaTime;

            float newAlpha = Mathf.Lerp(0f, 1f, timer / textFadeDuration);
            SetAlpha(text, newAlpha);
            yield return null;
        }
        SetAlpha(text, 1f);
    }

    private void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        if (text != null)
        {
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }
    }
}
