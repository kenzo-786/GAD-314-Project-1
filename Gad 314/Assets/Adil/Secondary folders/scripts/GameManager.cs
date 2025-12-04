using System;
using UnityEngine;

public enum GameState
{
    Gameplay,
    PetControl,
    Interaction,
    Loading,
    Paused,
    Cutscene
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentState = GameState.Gameplay;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        OnStateChanged?.Invoke(newState);

        if (newState == GameState.Gameplay || newState == GameState.PetControl)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public bool CanMove()
    {
        return CurrentState == GameState.Gameplay || CurrentState == GameState.PetControl;
    }
}
