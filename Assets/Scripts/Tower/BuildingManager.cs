using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Vector3 placementOffset;

    bool _isBuilding;
    TowerSO _activeTowerSO;
    GameObject _previewTowerInstance;
    Cell _cellOver;

    public static event Action StartBuilding;
    public static event Action SuccessBuilding;
    public static event Action CancelBuilding;

    void OnDestroy()
    {
        GridInput.CellClicked -= OnCellClicked;
        GridInput.CellOver -= OnCellOver;
        ButtonTower.TowerClicked -= OnTowerClicked;
    }

    void Start()
    {
        GridInput.CellClicked += OnCellClicked;
        GridInput.CellOver += OnCellOver;
        ButtonTower.TowerClicked += OnTowerClicked;
    }
    void OnCellOver(Cell cell)
    {
        _cellOver = cell;
        if (_previewTowerInstance != null)
        {
            _previewTowerInstance.transform.position = _cellOver.transform.position + placementOffset;
        }
    }

    void OnTowerClicked(TowerSO towerSO)
    {
        _activeTowerSO = towerSO;
        _isBuilding = towerSO != null;
        if (towerSO != null)
        {
            Vector3 position = _cellOver.transform.position + placementOffset;
            GameObject towerGo = PoolManager.Spawn(towerSO.prefab, position, Quaternion.identity);
            _previewTowerInstance = towerGo;
            StartBuilding?.Invoke();
        }
    }

    void OnCellClicked(Cell cell)
    {
        if (!_isBuilding) return;
        switch (cell.type)
        {
            case CellType.Ground:
            {
                _previewTowerInstance = null;
                _isBuilding = false;
                if (!GoldManager.SubtractGold(_activeTowerSO.levels[0].cost))
                {
                    CancelBuilding?.Invoke();
                    return;
                }
                cell.SetType(CellType.Building);
                SuccessBuilding?.Invoke();
                break;
            }
            case CellType.Road:
            case CellType.Building:
            default:
                break;
        }
    }
}
