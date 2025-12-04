using UnityEngine;
using UnityEngine.UI;

public class InteractionHUD : MonoBehaviour
{
    public static InteractionHUD Instance;

    [Header("UI References")]
    public GameObject container;
    public Image progressImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Hide();
    }

    public void Show()
    {
        if (container) container.SetActive(true);
    }

    public void Hide()
    {
        if (container) container.SetActive(false);
    }

    public void UpdateProgress(float fillAmount)
    {
        if (progressImage)
        {
            progressImage.fillAmount = fillAmount;
        }
    }
}
