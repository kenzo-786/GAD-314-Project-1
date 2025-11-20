using UnityEngine;

public class ClickSFX : MonoBehaviour
{
    public AudioSource click;

    public void PlaySound()
    {
        click.Play();
    }
}
