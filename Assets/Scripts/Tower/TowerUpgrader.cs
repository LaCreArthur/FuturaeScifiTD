using UnityEngine;

public class TowerUpgrader : MonoBehaviour
{
    public Transform turretBase;

    int level;


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
}
