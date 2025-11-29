using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;

    private void Awake()
    {
        if (iconImage == null)
        {
            Transform iconTrans = transform.Find("Icon");
            if (iconTrans) iconImage = iconTrans.GetComponent<Image>();
        }

        if (countText == null)
            countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(ItemData item, int count)
    {
        if (iconImage)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            iconImage.preserveAspect = true;
        }

        if (countText)
        {
            countText.text = count > 1 ? count.ToString() : "";
        }
    }

    public void Clear()
    {
        if (iconImage)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText)
        {
            countText.text = "";
        }
    }
}
