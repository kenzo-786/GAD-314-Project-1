using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("DeveloperMenu");
    }

    // Called when Quit button is clicked
    public void ExitGame()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }

    // Called when Credits button is clicked
    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }
}