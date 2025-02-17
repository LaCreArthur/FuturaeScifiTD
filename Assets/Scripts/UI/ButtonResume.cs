using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonResume : MonoBehaviour
{
    Button _button;
    public static event Action OnResumeButtonClicked;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OnResumeButtonClicked?.Invoke());
    }
}
