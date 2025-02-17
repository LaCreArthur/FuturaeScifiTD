using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int s_score;
    static int s_highScore;

    [SerializeField] float timeToIncreaseScore = 1f;

    Coroutine _scoreCoroutine;

    public static event Action<int> OnScoreUpdated;
    public static int Score
    {
        get => s_score;
        private set {
            s_score = value;
            OnScoreUpdated?.Invoke(value);
            if (value > HighScore)
                HighScore = value;
        }
    }

    public static int HighScore
    {
        // if s_highScore is 0, get the high score from PlayerPrefs
        get => s_highScore == 0 ? PlayerPrefs.GetInt("highScore", 0) : s_highScore;
        private set {
            s_highScore = value;
            PlayerPrefs.SetInt("highScore", value);
        }
    }

    void Start()
    {
        Score = 0;
        GameStateManager.OnPlaying += StartIncreasingScore;
        GameStateManager.OnGameOver += StopIncreasingScore;
    }

    void OnDestroy()
    {
        GameStateManager.OnPlaying -= StartIncreasingScore;
        GameStateManager.OnGameOver -= StopIncreasingScore;
    }

    void StartIncreasingScore()
    {
        Score = 0;
        if (_scoreCoroutine != null)
            StopIncreasingScore();
        _scoreCoroutine = StartCoroutine(IncreaseScoreRoutine());
    }

    IEnumerator IncreaseScoreRoutine()
    {
        while (true)
        {
            Score++;
            yield return new WaitForSeconds(timeToIncreaseScore);
        }
    }

    void StopIncreasingScore() => StopCoroutine(_scoreCoroutine);
}
