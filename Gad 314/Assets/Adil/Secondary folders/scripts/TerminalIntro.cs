using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TerminalIntro : MonoBehaviour
{
    [Header("Configuration")]
    public string nextSceneName = "Level1_Lab";
    public float typingSpeed = 0.05f;
    public float lineDelay = 1.0f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip explosionSound;
    public AudioClip alarmSound;
    public AudioClip petVoiceSound;

    [Range(0f, 1f)]
    public float typingVolume = 0.3f;

    [Range(0f, 1f)]
    public float petVoiceVolume = 0.2f;

    [Header("UI References")]
    public TextMeshProUGUI terminalText;
    public GameObject blackScreen;

    private string[] _introLines =
    {
        "SYSTEM BOOT... [OK]",
        "USER - DR. STILINSKI",
        "DATE - 2125 - PROJECT X",
        "--------------------------------",
        "LOG ENTRY 10100 -",
        "",
        "We are close. The new material...",
        "It's unstable, but the energy output is infinite.",
        "If we can stabilize the Core, we can...",
        "Wait...",
        "Energy spike detected.",
        "Stabilizers failing...",
        "--------------------------------",
        "WARNING - CRITICAL ERROR",
        "CONTAINMENT BREACH DETECTED"
    };

    private string[] _postExplosionLines =
    {
        "SYSTEM REBOOT...",
        "--------------------------------",
        "Scanning for life signs...",
        "[1] Survivor Detected.",
        "Doctor? Can you hear me?",
        "The blast... it released a virus.",
        "The world is changing rapidly.",
        "We are left with one option.",
        "I have located the resources in the Dino Era.",
        "We must travel back.",
        "Find the cure.",
        "Save the future.",
        "--------------------------------",
        "Initiating Time Jump..."
    };

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator PlaySequence()
    {
        terminalText.text = "";

        foreach (string line in _introLines)
        {
            yield return StartCoroutine(TypeText(line));
            yield return new WaitForSeconds(lineDelay);

            if (line.Contains("WARNING"))
            {
                terminalText.color = Color.red;
                if (alarmSound)
                    audioSource.PlayOneShot(alarmSound);
            }
        }

        yield return new WaitForSeconds(0.5f);

        terminalText.text = "";

        if (explosionSound)
            audioSource.PlayOneShot(explosionSound);

        terminalText.color = Color.red;
        terminalText.text = "SIGNAL LOST";

        yield return new WaitForSeconds(0.1f);

        terminalText.text = "";

        yield return new WaitForSeconds(0.1f);

        terminalText.text = "ERROR";

        yield return new WaitForSeconds(2.0f);

        terminalText.color = Color.black;
        terminalText.text = "";

        foreach (string line in _postExplosionLines)
        {
            if (petVoiceSound)
                audioSource.PlayOneShot(petVoiceSound, petVoiceVolume);

            yield return StartCoroutine(TypeText(line));
            yield return new WaitForSeconds(lineDelay);
        }

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator TypeText(string line)
    {
        terminalText.text += "\n";

        foreach (char c in line)
        {
            terminalText.text += c;

            if (typingSound)
                audioSource.PlayOneShot(typingSound, typingVolume);

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
