using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public bool stackable = true;
    public int maxStack = 64;
}
