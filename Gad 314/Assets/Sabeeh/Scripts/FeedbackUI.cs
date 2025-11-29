using UnityEngine;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    public TMP_Text feedbackText;

    public void ShowMessage(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
        }
    }

    public void HideMessage()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }
}

