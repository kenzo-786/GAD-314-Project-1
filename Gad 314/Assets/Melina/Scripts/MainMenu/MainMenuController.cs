using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject volumePanel;
    public Slider volumeSlider;

    void Start()
    {
        creditsPanel.SetActive(false);
        volumePanel.SetActive(false);
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void OpenVolume()
    {
        volumePanel.SetActive(true);
    }

    public void CloseVolume()
    {
        volumePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnVolumeChange(float value)
    {
        AudioListener.volume = value;
    }
}