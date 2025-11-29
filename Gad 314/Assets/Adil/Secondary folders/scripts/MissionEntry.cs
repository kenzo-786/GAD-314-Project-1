using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionEntry : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI missionText;
    public Image checkboxImage; 

    [Header("Visual Settings")]
    public Sprite untickedSprite;
    public Sprite tickedSprite;
    public Color completedColor = Color.gray;

    public void Setup(string text, Sprite emptyBox, Sprite checkedBox)
    {
        missionText.text = text;
        untickedSprite = emptyBox;
        tickedSprite = checkedBox;

        checkboxImage.sprite = untickedSprite;
        missionText.fontStyle = FontStyles.Normal;
        missionText.color = Color.white;
    }

    public void UpdateText(string newText)
    {
        if (checkboxImage.sprite == untickedSprite)
        {
            missionText.text = newText;
        }
    }

    public void SetComplete()
    {
        if (tickedSprite != null)
            checkboxImage.sprite = tickedSprite;

        missionText.fontStyle = FontStyles.Strikethrough;

        missionText.color = completedColor;
    }

}
