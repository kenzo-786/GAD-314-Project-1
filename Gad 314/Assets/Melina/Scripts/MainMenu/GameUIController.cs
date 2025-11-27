using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject controlsPanel;
    public GameObject pauseButton;

    void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void TogglePause()
    {
        if (!pausePanel.activeSelf && !controlsPanel.activeSelf)
        {
            pausePanel.SetActive(true);
            pauseButton.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            controlsPanel.SetActive(false);
            pauseButton.SetActive(true);
            Time.timeScale = 1f;
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
}