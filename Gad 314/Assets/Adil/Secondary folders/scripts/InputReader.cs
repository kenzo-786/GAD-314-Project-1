using UnityEngine;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance;

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

    public Vector2 GetMoveInput()
    {
        if (GameManager.Instance == null) return Vector2.zero;

        if (!GameManager.Instance.CanMove()) return Vector2.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y);
    }

    public bool GetJumpDown()
    {
        if (!GameManager.Instance.CanMove()) return false;
        return Input.GetButtonDown("Jump");
    }

    public bool GetSprintHeld()
    {
        if (!GameManager.Instance.CanMove()) return false;
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool GetDashDown()
    {
        if (!GameManager.Instance.CanMove()) return false;
        return Input.GetKeyDown(KeyCode.R);
    }

    public bool GetInteractHeld()
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return false;

        return Input.GetKey(KeyCode.E);
    }

    public bool GetSwitchPetDown()
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay &&
           GameManager.Instance.CurrentState != GameState.PetControl)
            return false;

        return Input.GetKeyDown(KeyCode.Tab);
    }
}
