using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTower : MonoBehaviour
{
    [SerializeField] TowerSO towerSO;
    [SerializeField] TMP_Text nameTmp;
    [SerializeField] TMP_Text priceTmp;
    [SerializeField] Image iconImage;
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
        iconImage.sprite = towerSO.icon;
        nameTmp.text = towerSO.name;
        priceTmp.text = towerSO.levels[0].cost.ToString();
    }

    void OnDestroy() => BuildingManager.StopBuilding -= OnStopBuilding;

    void OnStopBuilding() => SetSelected(false);

    void OnClick()
    {
        SetSelected(!_selected);
        TowerClicked?.Invoke(_selected ? towerSO?.prefab : null);
    }

    void SetSelected(bool value)
    {
        _selected = value;
        _button.image.color = _selected ? selectedColor : defaultColor;
    }
}
