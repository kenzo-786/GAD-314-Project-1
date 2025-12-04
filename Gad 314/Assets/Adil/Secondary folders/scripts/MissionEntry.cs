using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionEntry : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI missionText;
    public Image checkboxImage; 

    [Header("Visual Settings")]
    public Sprite emptyBox;
    public Sprite checkedBox;
    public Color normalColor = Color.white;
    public Color completedColor = Color.gray;

    public void Setup(string text, Sprite emptySprite, Sprite checkedSprite)
    {
        missionText.text = text;

        if (emptySprite != null) emptyBox = emptySprite;
        if (checkedSprite != null) checkedBox = checkedSprite;

        checkboxImage.sprite = emptyBox;
        missionText.color = normalColor;
        missionText.fontStyle = FontStyles.Normal;
    }

    public void UpdateText(string newText)
    {
        if (checkboxImage.sprite == emptyBox)
        {
            missionText.text = newText;
        }
    }

    public void SetComplete()
    {
        if (checkedBox != null) checkboxImage.sprite = checkedBox;

        missionText.fontStyle = FontStyles.Strikethrough;

        missionText.color = completedColor;


    }

}
