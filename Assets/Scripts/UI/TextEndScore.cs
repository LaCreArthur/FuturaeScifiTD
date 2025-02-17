using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextEndScore : MonoBehaviour
{
    TextMeshProUGUI _text;
    void Awake() => _text = GetComponent<TextMeshProUGUI>();
    void OnEnable() => UpdateScoreText();
    void UpdateScoreText() => _text.text =
        ScoreManager.Score == ScoreManager.HighScore ? $"New high score: {ScoreManager.HighScore}!" :
            $"Score: {ScoreManager.Score}\nHigh score: {ScoreManager.HighScore}";
}
