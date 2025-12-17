using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class AccessibilityManager : MonoBehaviour
{
    public static AccessibilityManager Instance;

    [Header("Settings")]
    public bool motionSicknessDotEnabled = false;
    public KeyCode toggleKey = KeyCode.F2;

    [Header("Grid Generation")]
    public int rows = 5;
    public int columns = 9;
    public float spacing = 150f;
    public GameObject dotPrefab;

    [Header("References")]
    public GameObject dotContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateDotGrid();
        UpdateDotVisibility();

        if (GameManager.Instance)
        {
            GameManager.Instance.OnStateChanged += OnStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnStateChanged -= OnStateChanged;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            motionSicknessDotEnabled = !motionSicknessDotEnabled;
            UpdateDotVisibility();
        }
    }

    private void OnStateChanged(GameState newState)
    {
        UpdateDotVisibility();
    }

    private void UpdateDotVisibility()
    {
        if (dotContainer == null) return;

        bool shouldShow = motionSicknessDotEnabled &&
                          GameManager.Instance &&
                          GameManager.Instance.CurrentState == GameState.PetControl;

        dotContainer.SetActive(shouldShow);
    }

    private void GenerateDotGrid()
    {
        if (dotContainer == null) return;

        foreach (Transform child in dotContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = -columns / 2; x <= columns / 2; x++)
        {
            for (int y = -rows / 2; y <= rows / 2; y++)
            {
                CreateDot(x * spacing, y * spacing);
            }
        }
    }

    private void CreateDot(float x, float y)
    {
        GameObject dot;
        if (dotPrefab != null)
        {
            dot = Instantiate(dotPrefab, dotContainer.transform);
        }
        else
        {
            dot = new GameObject("Dot", typeof(Image));
            dot.transform.SetParent(dotContainer.transform, false);

            Image img = dot.GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 0.5f);

            RectTransform rt = dot.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(4, 4);
        }

        RectTransform rect = dot.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(x, y);
        }
    }


}
