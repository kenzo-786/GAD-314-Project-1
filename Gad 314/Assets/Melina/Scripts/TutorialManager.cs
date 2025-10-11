using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; 
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTutorial();
        }
    }

    void ToggleTutorial()
    {
        isOpen = !isOpen;
        tutorialPanel.SetActive(isOpen);
    }
}