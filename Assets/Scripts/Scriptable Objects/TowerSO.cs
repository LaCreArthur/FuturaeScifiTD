using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Create TowerSO", fileName = "TowerSO", order = 0)]
public class TowerSO : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public TowerLevel[] levels;
}

[Serializable]
public struct TowerLevel
{
    public int damage;
    public float range;
    public float fireRate;
    public int cost;
}
