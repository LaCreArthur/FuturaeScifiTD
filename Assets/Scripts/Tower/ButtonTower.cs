using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTower : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;
    Button _button;

    bool _selected;

    public static event Action<GameObject> TowerClicked;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        BuildingManager.StopBuilding += OnStopBuilding;
    }

    void OnDestroy() => BuildingManager.StopBuilding -= OnStopBuilding;

    void OnStopBuilding() => SetSelected(false);

    void OnClick()
    {
        SetSelected(!_selected);
        TowerClicked?.Invoke(_selected ? towerPrefab : null);
    }
    void SetSelected(bool value)
    {
        _selected = value;
        _button.image.color = _selected ? selectedColor : defaultColor;
    }
}
