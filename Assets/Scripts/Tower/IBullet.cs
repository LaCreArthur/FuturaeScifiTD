using UnityEngine;

public interface IBullet
{
    void Initialize(Transform tower, Transform target, TowerSO towerSO);
}
