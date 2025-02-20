using UnityEngine;

public interface IBullet
{
    void Initialize(Tower tower, Transform target, TowerSO towerSO);
}
