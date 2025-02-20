using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextGameScore : MonoBehaviour
{
    TextMeshProUGUI _text;
    void Awake() => _text = GetComponent<TextMeshProUGUI>();
    void OnDestroy() => ScoreManager.OnScoreUpdated -= UpdateScoreText;
    void Start() => ScoreManager.OnScoreUpdated += UpdateScoreText;
    void UpdateScoreText(int score) => _text.text = $"{score}";
}
