using UnityEngine;

public class ButtonCredits : MonoBehaviour
{
    public GameObject credits;

    public void OpenCredits()
    {
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }
}
