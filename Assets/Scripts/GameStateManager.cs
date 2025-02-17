using System;
using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static Action OnHome, OnPlaying, OnGameOver, OnLeaderboard;
    public static Action<GameState> StateChange;
    public static GameState CurrentState { get; private set; }

    void Awake() => CurrentState = GameState.Loading;

    IEnumerator Start()
    {
        // delay set state to Home so that all the Awake and Start methods are called and all the events are subscribed
        yield return new WaitForEndOfFrame();
        SetState(GameState.Home);
    }

    public static void SetState(GameState newState)
    {
        if (newState == CurrentState)
        {
            Debug.LogWarning("Trying to set the same state as the current one.");
            return;
        }

        // If we're pausing the game, set IsGamePaused; otherwise reset it
        CurrentState = newState;
        StateChange?.Invoke(CurrentState);
        Debug.Log($"Game State changed to {newState}");

        // Invoke the corresponding event
        switch (newState)
        {
            case GameState.Home:
                OnHome?.Invoke();
                break;
            case GameState.Playing:
                OnPlaying?.Invoke();
                break;
            case GameState.GameOver:
                OnGameOver?.Invoke();
                break;
            case GameState.Leaderboard:
                OnLeaderboard?.Invoke();
                break;
        }
    }
}

public enum GameState
{
    Loading,
    Home,
    Playing,
    GameOver,
    Leaderboard,
}
