using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject controlsPanel;
    public GameObject pauseButton;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        pauseButton.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        controlsPanel.SetActive(false);
        pauseButton.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OpenControls()
    {
        controlsPanel.SetActive(true);
        pausePanel.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void BackFromControls()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void ToggleCursor()
    {
        if (!isPaused)
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}