using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PetSwitcher : MonoBehaviour
{
    [Header("Game Design Settings")]
    public string dinoSceneName = "FinalMasterScene";
    public bool canSwitch = false;
    public string disabledMessage = "Signal Blocked: Cannot switch in Lab.";

    [Header("Cameras")]
    public GameObject mainCameraObject;
    public GameObject petCameraObject;

    [Header("References")]
    public Transform playerCameraTarget;
    public TextMeshProUGUI errorTextUI;

    [Header("Settings")]
    public float switchRadius = 5f;

    private Transform _playerTransform;
    private Transform _petTransform;
    private PetController _petController;
    private float _uiTimer;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChange;
    }

    private void HandleStateChange(GameState newState)
    {
        if (newState == GameState.Gameplay)
        {
            if (mainCameraObject) mainCameraObject.SetActive(true);
            if (petCameraObject) petCameraObject.SetActive(false);
            if (_petController) _petController.isFirstPerson = false;
        }
        else if (newState == GameState.PetControl)
        {
            if (mainCameraObject) mainCameraObject.SetActive(false);
            if (petCameraObject) petCameraObject.SetActive(true);
            if (_petController) _petController.isFirstPerson = true;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PetSwitcher] New Scene Loaded: {scene.name}. Re-linking references...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) _playerTransform = player.transform;

        _petController = FindFirstObjectByType<PetController>();
        if (_petController)
        {
            _petTransform = _petController.transform;

            Camera petCam = _petController.GetComponentInChildren<Camera>(true);
            if (petCam) petCameraObject = petCam.gameObject;
        }

        if (Camera.main) mainCameraObject = Camera.main.gameObject;

        GameObject uiTextObj = GameObject.Find("ErrorText");
        if (uiTextObj) errorTextUI = uiTextObj.GetComponent<TextMeshProUGUI>();

        if (scene.name == dinoSceneName)
        {
            canSwitch = true;
            Debug.Log("[PetSwitcher] Signal Established: Switching Enabled.");
        }
        else
        {
            canSwitch = false;
        }

    }

    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

    }

    private void Update()
    {
        if (_uiTimer > 0)
        {
            _uiTimer -= Time.deltaTime;
            if (_uiTimer <= 0 && errorTextUI) errorTextUI.text = "";
        }

        if (InputReader.Instance.GetSwitchPetDown())
        {
            AttemptSwitch();
        }
    }

    private void AttemptSwitch()
    {
        if (!canSwitch)
        {
            ShowError(disabledMessage);
            return;
        }

        if (_petTransform == null || !_petTransform.gameObject.activeInHierarchy)
        {
            _petController = FindFirstObjectByType<PetController>();
            if (_petController) _petTransform = _petController.transform;

            ShowError("No Signal: Pet not found.");
            return;
        }

        if (_petController != null && _petController.isBroken)
        {
            ShowError("System Failure: Pet requires reboot.");
            return;
        }

        ToggleControl();
    }

    private void ShowError(string message)
    {
        Debug.Log(message);
        if (errorTextUI)
        {
            errorTextUI.text = message;
            _uiTimer = 2.0f;
        }
    }

    private void ToggleControl()
    {
        GameState current = GameManager.Instance.CurrentState;

        if (current == GameState.Gameplay)
        {
            GameManager.Instance.SetState(GameState.PetControl);
        }

        else if (current == GameState.PetControl)
        {

            float dist = Vector3.Distance(_playerTransform.position, _petTransform.position);

            if (dist <= switchRadius)
            {
                GameManager.Instance.SetState(GameState.Gameplay);
            }
            else
            {
                ShowError("Signal Weak: Get closer to Scientist.");
            }
        }
    }
}
