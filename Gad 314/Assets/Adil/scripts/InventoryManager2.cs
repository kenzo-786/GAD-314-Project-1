using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager2 : MonoBehaviour
{
  
    public static InventoryManager2 Instance;
    public List<Item> items = new List<Item>();

    public Transform ItemContent;
    public GameObject InvItem;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        if (items.Count > 0)
        {
            ListItems();
        }
    }
    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform child in ItemContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items) 
        {
            GameObject obj = Instantiate(InvItem, ItemContent);
            var nameTransform = obj.transform.Find("Item name");
            var iconTransform = obj.transform.Find("Image");

            if (nameTransform == null || iconTransform == null)
            {
                Debug.LogError("missing name/prefab");
                continue;
            }

            var ItemName = nameTransform.GetComponent<Text>();
            var ItemIcon = iconTransform.GetComponent<Image>();

            ItemName.text = item.itemName;
            ItemIcon.sprite = item.icon;

        }
    }
}
