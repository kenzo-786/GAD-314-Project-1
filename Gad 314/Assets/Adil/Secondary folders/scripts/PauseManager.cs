using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject accessibilityPanel;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused;

    private void Start()
    {
        SetAllPanels(false);
        ResumeGame();
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        accessibilityPanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameState.Paused);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        SetAllPanels(false);

        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameState.Gameplay);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenControls()
    {
        pauseMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
        accessibilityPanel.SetActive(false);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void OpenAccessibility()
    {
        pauseMenuPanel.SetActive(false);
        accessibilityPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void CloseAccessibility()
    {
        accessibilityPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void SetAllPanels(bool state)
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(state);
        if (controlsPanel != null) controlsPanel.SetActive(state);
        if (accessibilityPanel != null) accessibilityPanel.SetActive(state);
    }
}
