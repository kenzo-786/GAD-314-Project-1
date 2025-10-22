using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void BeginGame()
    {
        SceneManager.LoadScene("PatrollingEnemy");
    }
}
