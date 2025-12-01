using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Configuration")]
    public string targetSceneName;
    public string missionID_ToComplete = "";

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float holdTime = 1.5f;
    public AudioClip teleportSound;

    private bool _inZone = false;
    private float _timer = 0f;
    private bool _isTeleporting = false;

    private void Update()
    {
        if (_isTeleporting || !_inZone) return;

        if (Input.GetKey(interactKey))
        {
            _timer += Time.deltaTime;

            if (InteractionHUD.Instance)
                InteractionHUD.Instance.UpdateProgress(_timer / holdTime);

            if (_timer >= holdTime)
            {
                StartTeleportSequence();
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

    private void StartTeleportSequence()
    {
        _isTeleporting = true;

        if (teleportSound) AudioSource.PlayClipAtPoint(teleportSound, transform.position);

        if (!string.IsNullOrEmpty(missionID_ToComplete))
        {
            if (MissionManager.Instance != null)
            {
                Debug.Log($"[Teleporter] Completing Mission: {missionID_ToComplete}");
                MissionManager.Instance.CompleteMission(missionID_ToComplete);
            }
            else
            {
                Debug.LogWarning("[Teleporter] Warning: Mission ID set, but MissionManager is missing!");
            }
        }

        if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();

        if (GameManager.Instance) GameManager.Instance.SetState(GameState.Loading);

        if (LevelLoader.Instance)
        {
            LevelLoader.Instance.LoadLevel(targetSceneName);
        }
        else
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inZone = true;
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
            _inZone = false;
            _timer = 0;
            if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();
        }
    }
}

  
