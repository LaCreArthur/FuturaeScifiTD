using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Vector3 placementOffset;
    [SerializeField] Material defaultMat;
    [SerializeField] Material previewMat;

    bool _isBuilding;
    GameObject _activeTowerPrefab;
    GameObject _previewTowerInstance;
    Cell _cellOver;

    public static event Action StartBuilding;
    public static event Action Built;

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
            _previewTowerInstance = SpawnTower(_cellOver.transform.position + placementOffset, previewMat);
            StartBuilding?.Invoke();
        }
    }

    GameObject SpawnTower(Vector3 position, Material mat)
    {
        GameObject go = PoolManager.Spawn(_activeTowerPrefab, position, Quaternion.identity);
        SetMaterial(go, mat);
        return go;
    }

    static void SetMaterial(GameObject go, Material mat)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material = mat;
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
                SetMaterial(_previewTowerInstance, defaultMat);
                _previewTowerInstance = null;
                _isBuilding = false;
                Built?.Invoke();
                break;
            }
            case CellType.Road:
            case CellType.Building:
            default:
                break;
        }
    }
}
