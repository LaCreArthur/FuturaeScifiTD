using UnityEngine;

public class PoolablePrefabRef : MonoBehaviour, IPoolable
{
    public GameObject Prefab;
    public void OnSpawn() {}
}
