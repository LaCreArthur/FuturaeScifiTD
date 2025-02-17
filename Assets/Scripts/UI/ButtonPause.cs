using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonPause : MonoBehaviour
{

    Button _button;
    public static event Action OnPauseButtonClicked;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OnPauseButtonClicked?.Invoke());
    }
}
