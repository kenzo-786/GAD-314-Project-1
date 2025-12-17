using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject controlsPanel;
    public GameObject accessibilityPanel;

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public KeyCode pauseKey = KeyCode.Escape;

    private bool _isPaused = false;

    private void Start()
    {
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (controlsPanel) controlsPanel.SetActive(false);
        if (accessibilityPanel) accessibilityPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance)
        {
            GameState state = GameManager.Instance.CurrentState;
            if (state != GameState.Gameplay && state != GameState.PetControl && state != GameState.Paused) return;
        }

        if (Input.GetKeyDown(pauseKey))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel) pauseMenuPanel.SetActive(true);

        if (GameManager.Instance)
        {
            GameManager.Instance.SetState(GameState.Paused);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (controlsPanel) controlsPanel.SetActive(false);
        if (accessibilityPanel) accessibilityPanel.SetActive(false);

        if (GameManager.Instance)
        {
            GameManager.Instance.SetState(GameState.Gameplay);
        }
    }

    public void OpenControls()
    {
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (controlsPanel) controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        if (controlsPanel) controlsPanel.SetActive(false);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
    }

    public void OpenAccessibility()
    {
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (accessibilityPanel) accessibilityPanel.SetActive(true);
    }

    public void CloseAccessibility()
    {
        if (accessibilityPanel) accessibilityPanel.SetActive(false);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
