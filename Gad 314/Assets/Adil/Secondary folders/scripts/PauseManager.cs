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

    private bool isPaused = false;
    private GameState previousState;

    private void Start()
    {
        SetAllPanels(false);
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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
        if (GameManager.Instance == null) return;

        isPaused = true;
        Time.timeScale = 0f;

        previousState = GameManager.Instance.CurrentState;

        pauseMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        accessibilityPanel.SetActive(false);

        GameManager.Instance.SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (GameManager.Instance == null) return;

        isPaused = false;
        Time.timeScale = 1f;

        SetAllPanels(false);

        GameManager.Instance.SetState(previousState);
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

