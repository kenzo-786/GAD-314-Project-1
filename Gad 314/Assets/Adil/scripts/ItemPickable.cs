using UnityEngine;

public class ItemPickable : MonoBehaviour , IPickable
{
    public ItemList itemScriptableObject;

    public void PickItem()
    {
        Destroy(gameObject);
    }
}
