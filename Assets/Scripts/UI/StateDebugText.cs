using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StateDebugText : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GameStateManager.StateChange += UpdateStateText;
    }

    void OnDestroy() => GameStateManager.StateChange -= UpdateStateText;

    void UpdateStateText(GameState state) => _text.text = $"State: {state}";
}
