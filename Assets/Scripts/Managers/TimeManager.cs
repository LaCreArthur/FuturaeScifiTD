using UnityEngine;

public class TimeManager : MonoBehaviour
{
    void Start()
    {
        GameStateManager.OnPlaying += ResumeTime;
        ButtonResume.OnResumeButtonClicked += ResumeTime;
        ButtonPause.OnPauseButtonClicked += PauseTime;
        LevelUpChoiceUI.UpgradeChosen += ResumeTime;
    }

    void OnDestroy()
    {
        GameStateManager.OnPlaying -= ResumeTime;
        ButtonResume.OnResumeButtonClicked -= ResumeTime;
        ButtonPause.OnPauseButtonClicked -= PauseTime;
        LevelUpChoiceUI.UpgradeChosen -= ResumeTime;
    }

    static void PauseTime() => Time.timeScale = 0;
    static void ResumeTime() => Time.timeScale = 1;
}
