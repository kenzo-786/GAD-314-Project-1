using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 1;
            PausePanel.SetActive(false);
        }
    }
}
