using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string id;
    public string displayName;

    [TextArea] public string description;
    public Sprite icon;

    [Header("Behavior")]
    public bool isStackable = true;
    public int maxStack = 64;
}
