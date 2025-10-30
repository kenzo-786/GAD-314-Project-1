using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when Play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene("SceneOne");
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
 }
}
