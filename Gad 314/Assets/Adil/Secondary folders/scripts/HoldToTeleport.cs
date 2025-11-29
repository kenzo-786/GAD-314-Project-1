using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldToTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]

    public string sceneToLoad;
    public string missionToComplete = "";

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float holdDuration = 2.0f;

    [Header("Visuals/Audio")]
    public AudioClip teleportSound;

    private bool _isPlayerInRange = false;
    private float _currentHoldTime = 0f;

    private void Update()
    {
        if (_isPlayerInRange)
        {
            if (Input.GetKey(interactKey))
            {
                _currentHoldTime += Time.deltaTime;

                if (InteractionHUD.Instance != null)
                {
                    float progress = _currentHoldTime / holdDuration;
                    InteractionHUD.Instance.UpdateProgress(progress);
                }

                if (_currentHoldTime >= holdDuration)
                {
                    PerformTeleport();
                }
            }
            else
            {
                if (_currentHoldTime > 0)
                {
                    _currentHoldTime = 0;
                    if (InteractionHUD.Instance != null)
                        InteractionHUD.Instance.UpdateProgress(0);
                }
            }
        }
    }

    private void PerformTeleport()
    {
        if (teleportSound) AudioSource.PlayClipAtPoint(teleportSound, transform.position);

        if (!string.IsNullOrEmpty(missionToComplete))
        {
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.CompleteMission(missionToComplete);
            }
        }

        if (InteractionHUD.Instance != null) InteractionHUD.Instance.Hide();

        if (LevelLoader.Instance != null)
        {
            LevelLoader.Instance.LoadLevel(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            if (InteractionHUD.Instance != null)
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
            _isPlayerInRange = false;
            _currentHoldTime = 0;
            if (InteractionHUD.Instance != null)
            {
                InteractionHUD.Instance.Hide();
            }
        }
    }
}
