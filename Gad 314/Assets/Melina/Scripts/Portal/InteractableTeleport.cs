using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InteractableTeleport : MonoBehaviour 
{
    public string sceneToLoad;
    public float holdTime = 2f;
    public TextMeshProUGUI promptText;

    private bool playerNear = false;
    private float holdTimer = 0f;

    private void Start()
    {
        promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerNear)
        {
            if (Input.GetKey(KeyCode.T))
            {
                holdTimer += Time.deltaTime;
                promptText.text = "Holding T... " + (holdTimer / holdTime * 100f).ToString("F0") + "%";

                if (holdTimer >= holdTime)
                {
                    Teleport();
                }
            }
            else
            {
                holdTimer = 0f;
                promptText.text = "Hold T to Teleport";
            }
        }
    }

    private void Teleport()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            promptText.text = "Hold T to Teleport";
            promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            holdTimer = 0f;
            promptText.gameObject.SetActive(false);
        }
    }
}