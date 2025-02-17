using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Vector3 placementOffset;

    bool _isBuilding;
    GameObject _activeTower;


    void Start()
    {
        GridInput.CellClicked += OnCellClicked;
        ButtonTower.TowerClicked += OnTowerClicked;
    }

    void OnDestroy() => GridInput.CellClicked -= OnCellClicked;

    void OnTowerClicked(GameObject prefab)
    {
        _activeTower = prefab;
        _isBuilding = prefab != null;
    }

    void OnCellClicked(Cell cell)
    {
        if (!_isBuilding) return;
        switch (cell.type)
        {

            case CellType.Ground:
            {
                cell.SetType(CellType.Building);
                PoolManager.Spawn(_activeTower, cell.transform.position + placementOffset, Quaternion.identity);
            }
                break;
            case CellType.Road:
            case CellType.Building:
            default:
                break;
        }
    }
}
