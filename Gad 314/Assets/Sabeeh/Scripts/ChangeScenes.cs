using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void GoToSceneTwo()
    {
        SceneManager.LoadScene("SceneTwo");
    }
    public void GoToSceneThree()
    {
        SceneManager.LoadScene("SceneThree");
    }
}
