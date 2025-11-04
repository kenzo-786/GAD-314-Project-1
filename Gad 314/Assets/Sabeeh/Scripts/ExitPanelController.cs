using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPanelController : MonoBehaviour
{
    public GameObject exitPanel;   
    public Button yesButton;       

    private bool isPanelVisible = false;

    void Start()
    {
       
        exitPanel.SetActive(false);

       
       
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPanelVisible)
                HidePanel();
            else
                ShowPanel();
        }
    }

    void ShowPanel()
    {
        exitPanel.SetActive(true);
        isPanelVisible = true;

        
    }

    void HidePanel()
    {
        exitPanel.SetActive(false);
        isPanelVisible = false;

      
    }

    public void PlayGame()
    {


        Debug.Log("Pressed");
        SceneManager.LoadScene("DeveloperMenu");
    }
}
