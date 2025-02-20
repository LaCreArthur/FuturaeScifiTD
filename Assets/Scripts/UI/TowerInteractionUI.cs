using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInteractionUI : MonoBehaviour
{
    [SerializeField] Button upgradeButton;
    [SerializeField] Button sellButton;
    [SerializeField] TMP_Text upgradeCostText;
    [SerializeField] TMP_Text sellValueText;

    Tower _selectedTower;

    void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        sellButton.onClick.AddListener(OnSellClicked);
        gameObject.SetActive(false); // Hidden by default
    }

    public void Show(Tower tower)
    {
        _selectedTower = tower;
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);

    void UpdateUI()
    {
        int upgradeCost = _selectedTower.GetUpgradeCost();
        upgradeButton.interactable = upgradeCost > 0 && GoldManager.CanAfford(upgradeCost);
        upgradeCostText.text = upgradeCost > 0 ? $"{upgradeCost} Gold" : "Max Level";
        sellValueText.text = $"{_selectedTower.GetSellValue()} Gold";
    }

    void OnUpgradeClicked()
    {
        int cost = _selectedTower.GetUpgradeCost();
        if (GoldManager.SubtractGold(cost))
        {
            _selectedTower.Upgrade();
            Hide();
        }
    }

    void OnSellClicked()
    {
        GoldManager.AddGold(_selectedTower.GetSellValue());
        Vector2Int cellPosition = Grid.GetGridPos(_selectedTower.transform.position);
        Grid.Cells[cellPosition].SetType(CellType.Ground); // Reset cell
        PoolManager.Despawn(_selectedTower.gameObject);
        Hide();
    }
}
