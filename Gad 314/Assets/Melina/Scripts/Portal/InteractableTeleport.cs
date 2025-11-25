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
        Debug.Log("Start() called. promptText = " + (promptText == null ? "NULL" : "Assigned"));
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
                    Debug.Log("Teleport triggered");
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
        Debug.Log("Scene loading: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter called by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger");
            playerNear = true;
            promptText.text = "Hold T to Teleport";
            promptText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Entered object is NOT Player. Tag = " + other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit called by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left trigger");
            playerNear = false;
            holdTimer = 0f;
            promptText.gameObject.SetActive(false);
        }
    }
}
