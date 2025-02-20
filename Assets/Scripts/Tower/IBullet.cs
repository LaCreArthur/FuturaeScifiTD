using UnityEngine;

public interface IBullet
{
    public void Initialize(Transform tower, Transform target, TowerSO towerSO);
}
