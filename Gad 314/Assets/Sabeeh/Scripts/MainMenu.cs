using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load your main game scene
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings(GameObject settingsPanel)
    {
        settingsPanel.SetActive(true);
    }

    public void OpenCredits(GameObject creditsPanel)
    {
        creditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
