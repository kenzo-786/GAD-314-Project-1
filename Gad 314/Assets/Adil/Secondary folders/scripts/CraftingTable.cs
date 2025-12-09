using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CraftingTable : MonoBehaviour
{
    [Header("The Output")]
    public ItemData finalItem;
    public int finalItemAmount = 1;

    [Header("The Recipe")]
    public List<ItemRequirement> requiredItems;

    [System.Serializable]
    public class ItemRequirement
    {
        public ItemData item;
        public int amount;
    }

    [Header("Game Over / Win")]
    public string winSceneName;

    [Header("Interaction")]
    public float holdDuration = 2.0f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Feedback")]
    public AudioClip craftSound;
    public AudioClip failSound;

    private bool _inRange;
    private float _timer;

    private void Update()
    {
        if (_inRange)
        {
            if (Input.GetKey(interactKey))
            {
                _timer += Time.deltaTime;

                if (InteractionHUD.Instance)
                    InteractionHUD.Instance.UpdateProgress(_timer / holdDuration);

                if (_timer >= holdDuration)
                {
                    TryCraft();
                    _timer = 0;
                }
            }
            else
            {
                if (_timer > 0)
                {
                    _timer = 0;
                    if (InteractionHUD.Instance) InteractionHUD.Instance.UpdateProgress(0);
                }
            }
        }
    }

    private void TryCraft()
    {
        if (InventoryManager.Instance == null) return;

        foreach (var req in requiredItems)
        {
            if (!InventoryManager.Instance.HasItem(req.item, req.amount))
            {
                Debug.Log("Missing Ingredients: " + req.item.displayName);
                if (failSound) AudioSource.PlayClipAtPoint(failSound, transform.position);

                return;
            }
        }

        foreach (var req in requiredItems)
        {
            InventoryManager.Instance.RemoveItem(req.item, req.amount);
        }

        if (finalItem != null)
        {
            InventoryManager.Instance.AddItem(finalItem, finalItemAmount);
            Debug.Log("Crafting Successful: " + finalItem.displayName);
        }

        if (craftSound) AudioSource.PlayClipAtPoint(craftSound, transform.position);
        if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();

        if (!string.IsNullOrEmpty(winSceneName))
        {

            Debug.Log("Game Finished! Loading Win Scene...");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (LevelLoader.Instance)
                LevelLoader.Instance.LoadLevel(winSceneName);
            else
                SceneManager.LoadScene(winSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            if (InteractionHUD.Instance)
            {
                InteractionHUD.Instance.Show();
                InteractionHUD.Instance.UpdateProgress(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            _timer = 0;
            if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();
        }
    }
}
