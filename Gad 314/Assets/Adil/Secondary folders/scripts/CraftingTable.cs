using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class CraftingTable : MonoBehaviour
{
    [Header("Missions")]
    public string missionToComplete = "craft_cure";
    public string missionToStart = "";

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
    public float waitBeforeWinScene = 4.0f;

    [Header("Interaction")]
    public float holdDuration = 2.0f;
    public KeyCode interactKey = KeyCode.E;
    public bool oneTimeOnly = true;

    [Header("Feedback")]
    public AudioClip craftSound;
    public AudioClip failSound;
    private bool _hasCrafted = false;

    private bool _inRange;
    private float _timer;

    private void Update()
    {
        if (_inRange && !_hasCrafted)
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
            if (req.item == null) continue;
            if (!InventoryManager.Instance.HasItem(req.item, req.amount))
            {
                if (failSound) AudioSource.PlayClipAtPoint(failSound, transform.position);
                return;
            }
        }

        foreach (var req in requiredItems)
        {
            if (req.item != null)
                InventoryManager.Instance.RemoveItem(req.item, req.amount);
        }

        if (finalItem != null)
        {
            InventoryManager.Instance.AddItem(finalItem, finalItemAmount);
        }

        if (craftSound) AudioSource.PlayClipAtPoint(craftSound, transform.position);
        if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();

        if (MissionManager.Instance)
        {
            if (!string.IsNullOrEmpty(missionToComplete))
                MissionManager.Instance.CompleteMission(missionToComplete);
        }

        if (oneTimeOnly)
        {
            _hasCrafted = true;
        }

        if (!string.IsNullOrEmpty(winSceneName))
        {
            StartCoroutine(EndGameRoutine());
        }
    }

    private IEnumerator EndGameRoutine()
    {
        Debug.Log("Crafting complete. Waiting for cinematic pause...");

        if (GameManager.Instance) GameManager.Instance.SetState(GameState.Paused);

        yield return new WaitForSeconds(waitBeforeWinScene);

        Debug.Log("Loading Win Scene...");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(winSceneName);

    }
}
