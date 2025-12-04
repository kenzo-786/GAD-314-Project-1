using UnityEngine;

public class ControlsHelpUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject controlsPanel;
    public GameObject helpButton;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.F1;
    public bool startVisible = false;

    private bool _isOpen = false;

    private void Start()
    {
        _isOpen = startVisible;
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleVisibility();
        }
    }

    public void ToggleVisibility()
    {
        _isOpen = !_isOpen;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(_isOpen);
        }
    }
}
