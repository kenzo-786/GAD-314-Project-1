using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "Scriptable Objects/ItemList")]
public class ItemList : ScriptableObject
{
    public float cooldown;
    public itemType item_type;
    public Sprite itemImage;
}

public enum itemType { Flower, Egg, Amber }