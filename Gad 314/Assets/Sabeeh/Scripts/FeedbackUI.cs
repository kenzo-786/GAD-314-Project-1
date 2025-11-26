using UnityEngine;
using TMPro;
public class FeedbackUI : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;  // Use TMP instead of UI.Text

    void Start()
    {
        feedbackText.text = "";
    }

    public void ShowMessage(string message)
    {
        feedbackText.text = message;
    }

    public void HideMessage()
    {
        feedbackText.text = "";
    }
}
