using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [Header("Grid Settings")]
    public int rows = 8;
    public int columns = 12;
    public float waveSpeed = 0.05f;

    [Header("References")]
    public Canvas transitionCanvas;
    public GameObject boxPrefab;
    public Transform gridContainer;

    private Image[,] _gridImages;
    private WaitForSeconds _waveDelay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupGrid();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupGrid()
    {
        _waveDelay = new WaitForSeconds(waveSpeed);
        _gridImages = new Image[columns, rows];

        GridLayoutGroup layout = gridContainer.GetComponent<GridLayoutGroup>();
        if (layout == null) layout = gridContainer.gameObject.AddComponent<GridLayoutGroup>();

        float width = 1920f / columns;
        float height = 1080f / rows;
        layout.cellSize = new Vector2(width + 2, height + 2);
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = columns;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject box = Instantiate(boxPrefab, gridContainer);
                Image img = box.GetComponent<Image>();
                img.color = Color.black;
                img.enabled = false;
                _gridImages[x, y] = img;
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(TransitionSequence(sceneName));
    }

    private IEnumerator TransitionSequence(string sceneName)
    {
        for (int sum = 0; sum < rows + columns; sum++)
        {
            for (int x = 0; x < columns; x++)
            {
                int y = sum - x;
                if (y >= 0 && y < rows)
                {
                    _gridImages[x, y].enabled = true;
                }
            }
            yield return _waveDelay;
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.5f);

        for (int sum = 0; sum < rows + columns; sum++)
        {
            for (int x = 0; x < columns; x++)
            {
                int y = sum - x;
                if (y >= 0 && y < rows)
                {
                    _gridImages[x, y].enabled = false;
                }
            }
            yield return _waveDelay;
        }
    }
}
