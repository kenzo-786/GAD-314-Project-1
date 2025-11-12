using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item Item;

    void PickingUp()
    {
        InventoryManager2.Instance.Add(Item);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        PickingUp();
    }
}
