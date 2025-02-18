using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Vector3 placementOffset;

    bool _isBuilding;
    GameObject _activeTowerPrefab;
    GameObject _previewTowerInstance;
    Cell _cellOver;

    public static event Action StartBuilding;
    public static event Action StopBuilding;

    void Start()
    {
        GridInput.CellClicked += OnCellClicked;
        GridInput.CellOver += OnCellOver;
        ButtonTower.TowerClicked += OnTowerClicked;
    }

    void OnDestroy()
    {
        GridInput.CellClicked -= OnCellClicked;
        GridInput.CellOver -= OnCellOver;
        ButtonTower.TowerClicked -= OnTowerClicked;
    }
    void OnCellOver(Cell cell)
    {
        _cellOver = cell;
        if (_previewTowerInstance != null)
        {
            _previewTowerInstance.transform.position = _cellOver.transform.position + placementOffset;
        }
    }

    void OnTowerClicked(GameObject prefab)
    {
        _activeTowerPrefab = prefab;
        _isBuilding = prefab != null;
        if (prefab != null)
        {
            Vector3 position = _cellOver.transform.position + placementOffset;
            GameObject go = PoolManager.Spawn(_activeTowerPrefab, position, Quaternion.identity);
            _previewTowerInstance = go;
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
                cell.SetType(CellType.Building);
                _previewTowerInstance = null;
                _isBuilding = false;
                StopBuilding?.Invoke();
                break;
            }
            case CellType.Road:
            case CellType.Building:
            default:
                break;
        }
    }
}
