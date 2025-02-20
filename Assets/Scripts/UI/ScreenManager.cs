using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject homeScreen, gameScreen, pauseScreen, levelUpScreen, endScreen, leaderboardScreen;

    void Awake()
    {
        homeScreen.SetActive(true);
        gameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        endScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
    }

    void OnDestroy()
    {
        GameStateManager.OnPlaying -= ShowGameScreen;
        GameStateManager.OnGameOver -= ShowEndScreen;
        GameStateManager.OnHome -= ShowHomeScreen;
        ButtonPause.OnPauseButtonClicked -= ShowPauseScreen;
        ButtonResume.OnResumeButtonClicked -= ShowGameScreen;
        LevelUpChoiceUI.UpgradeChosen -= ShowGameScreen;
        GameStateManager.OnLeaderboard -= ShowLeaderboard;
    }

    void Start()
    {
        GameStateManager.OnPlaying += ShowGameScreen;
        GameStateManager.OnGameOver += ShowEndScreen;
        GameStateManager.OnHome += ShowHomeScreen;
        ButtonPause.OnPauseButtonClicked += ShowPauseScreen;
        ButtonResume.OnResumeButtonClicked += ShowGameScreen;
        LevelUpChoiceUI.UpgradeChosen += ShowGameScreen;
        GameStateManager.OnLeaderboard += ShowLeaderboard;
    }

    void ShowGameScreen()
    {
        homeScreen.SetActive(false);
        pauseScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        gameScreen.SetActive(true);
    }

    void ShowEndScreen()
    {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
    }

    void ShowHomeScreen()
    {
        homeScreen.SetActive(true);
        endScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
    }

    void ShowPauseScreen()
    {
        gameScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    void ShowLevelUpScreen(int _)
    {
        gameScreen.SetActive(false);
        levelUpScreen.SetActive(true);
    }

    void ShowLeaderboard()
    {
        homeScreen.SetActive(false);
        endScreen.SetActive(false);
        leaderboardScreen.SetActive(true);
    }
}
