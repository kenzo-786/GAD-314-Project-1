using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;

    public IEnumerator FadeOut(float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0, 1, t / duration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, t / duration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }
    }
}