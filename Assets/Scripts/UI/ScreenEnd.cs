using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEnd : MonoBehaviour
{
    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject savedTextGO;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button submitButton;

    // this OnEnable will be called each time this object is (re)enabled
    // we show the input panel only if the player is a new leader
    void OnEnable()
    {
        inputPanel.SetActive(Leaderboard.IsNewLeader());
        savedTextGO.SetActive(false);
    }
    void Start() => submitButton.onClick.AddListener(OnSubmit);

    void OnSubmit()
    {
        string playerName = inputField.text;
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            Leaderboard.AddEntry(new LeaderboardEntry { name = playerName, score = ScoreManager.Score });
        }
        inputPanel.SetActive(false);
        savedTextGO.SetActive(true);
    }
}
