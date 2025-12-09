using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonPop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animation Settings")]
    public float pressScale = 0.9f;
    public float releaseScale = 1.15f;
    public float settleScale = 1f;
    public float animSpeed = 0.08f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        StopAllCoroutines();
        StartCoroutine(ScaleTo(baseScale * pressScale));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(DoubleBounce());
    }

    IEnumerator DoubleBounce()
    {
        yield return ScaleTo(baseScale * releaseScale);
        yield return ScaleTo(baseScale * settleScale);
    }

    IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animSpeed;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.localScale = target;
    }
}