using System.Collections.Generic;
using UnityEngine;

public class PlayerInv : MonoBehaviour
{
    public List<itemType> inventoryList;
    public int itemSelect;
    public float playerReach;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItem;
    [SerializeField] KeyCode pickItem;

    [Space(20)]
    [Header("Item Gameobj")]
    [SerializeField] GameObject egg_item;
    [SerializeField] GameObject flower_item;
    [SerializeField] GameObject amber_item;

    [Space(20)]
    [Header("item Prefabs")]
    [SerializeField] GameObject egg_Prefab;
    [SerializeField] GameObject flower_Prefab;
    [SerializeField] GameObject amber_Prefab;

    [SerializeField] Camera Cam;
 
    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };

    private void Start()
    {
        itemSetActive.Add(itemType.Flower, flower_item);
        itemSetActive.Add(itemType.Amber, amber_item);
        itemSetActive.Add(itemType.Egg, egg_item);

        NewItemSelected();
    }

    private void Update()
    {

        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, playerReach) && Input.GetKey(pickItem))
        {
            IPickable item =hit.collider.GetComponent<IPickable>();
            if(item != null)
            {
                inventoryList.Add(hit.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                item.PickItem();
            }
        }



        if(Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count>0)
        {
            itemSelect = 0;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            itemSelect = 1;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            itemSelect = 2;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3)
        {
            itemSelect = 3;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5) && inventoryList.Count > 4)
        {
            itemSelect = 4;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6) && inventoryList.Count > 5)
        {
            itemSelect = 5;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7) && inventoryList.Count > 6)
        {
            itemSelect = 6;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8) && inventoryList.Count > 7)
        {
            itemSelect = 7;
            NewItemSelected();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha9) && inventoryList.Count > 8)
        {
            itemSelect = 8;
            NewItemSelected();
        }
    }

    private void NewItemSelected()
    {
        flower_item.SetActive(false);
        egg_item.SetActive(false);
        amber_item.SetActive(false);

        GameObject selectedItemGameobject = itemSetActive[inventoryList[itemSelect]];
        selectedItemGameobject.SetActive(true);
    }


}

public interface IPickable
{
    void PickItem();

}
