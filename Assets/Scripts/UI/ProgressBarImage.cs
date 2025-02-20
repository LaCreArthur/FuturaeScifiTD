using ScriptableVariables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBarImage : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] FloatVar currentValue;
    [SerializeField] FloatVar maxValue;
    [SerializeField] int decimals;

    Image _image;
    float _maxValue = 1;
    float _currentValue;

    void Awake() => _image = GetComponent<Image>();

    void OnDestroy()
    {
        currentValue.OnChanged -= SetValue;
        maxValue.OnChanged -= SetMaxValue;
    }

    void Start()
    {
        currentValue.OnChanged += SetValue;
        maxValue.OnChanged += SetMaxValue;
        SetMaxValue(maxValue.Value);
        SetValue(currentValue.Value);
    }

    void UpdateBar()
    {
        _image.fillAmount = _currentValue / _maxValue;
        text.text = $"{_currentValue.ToString($"N{decimals}")}/{_maxValue.ToString($"N{decimals}")}";
    }

    void SetMaxValue(float value)
    {
        _maxValue = value;
        UpdateBar();
    }

    void SetValue(float value)
    {
        _currentValue = value;
        UpdateBar();
    }
}
