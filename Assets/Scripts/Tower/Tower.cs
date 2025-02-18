using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform turretBase;
    [SerializeField] GameObject turretRange;

    int level;

    void Awake()
    {
        BuildingManager.StartBuilding += OnStartBuilding;
        BuildingManager.Built += OnBuilt;
    }

    void OnDestroy()
    {
        BuildingManager.StartBuilding -= OnStartBuilding;
        BuildingManager.Built -= OnBuilt;
    }
    void OnStartBuilding() => SetRangeActive(true);
    void OnBuilt() => SetRangeActive(false);


    [ContextMenu("Level Up")]
    void LevelUp()
    {
        if (turretBase == null || turretBase.childCount == 0) return;
        level = (level + 1) % turretBase.childCount;
        for (int i = 0; i < turretBase.childCount; i++)
        {
            turretBase.GetChild(i).gameObject.SetActive(i == level);
        }
    }

    public void SetRangeActive(bool active) => turretRange.SetActive(active);
}
