using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject controlsPanel;

    void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OpenControls()
    {
        controlsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void BackFromControls()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}