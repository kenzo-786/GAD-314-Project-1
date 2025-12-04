using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 5)]
    public string[] lines;

    public MonoBehaviour playerController; // drag your player script here

    private int index = 0;
    private bool isActive = true;

    void Start()
    {
        dialogueBox.SetActive(true);
        dialogueText.text = lines[index];

        playerController.enabled = false; // FREEZE PLAYER
    }

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueText.text = lines[index];
        }
        else
        {
            dialogueBox.SetActive(false);
            isActive = false;

            playerController.enabled = true; // UNFREEZE player
        }
    }
}




