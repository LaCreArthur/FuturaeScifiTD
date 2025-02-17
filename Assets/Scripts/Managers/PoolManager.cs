using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    static readonly Dictionary<GameObject, GameObjectPool> Pools = new Dictionary<GameObject, GameObjectPool>();

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!Pools.ContainsKey(prefab))
        {
            Pools[prefab] = new GameObjectPool(prefab, 3);
        }
        return Pools[prefab].Spawn(position, rotation, parent);
    }

    public static void Despawn(GameObject instance)
    {
        if (instance.TryGetComponent(out PoolablePrefabRef prefabRef))
        {
            if (prefabRef.Prefab == null)
            {
                Debug.LogWarning($"the PoolablePrefabRef component has a null prefab ref. (object: {instance.name}", instance);
                return;
            }
            if (Pools.TryGetValue(prefabRef.Prefab, out GameObjectPool pool))
                pool.Despawn(instance);
            else
                Debug.LogWarning($"Trying to despawn a game object that is not in the pool. (object: {instance.name}, pool: {prefabRef.Prefab.name}", instance);
        }
        else
            Debug.LogWarning($"Trying to despawn a game object that has not a PoolablePrefabRef component. (object: {instance.name}", instance);
    }
}

public class GameObjectPool
{
    readonly List<GameObject> _activeObjects = new List<GameObject>();
    readonly List<GameObject> _inactiveObjects = new List<GameObject>();
    // Keep a reference to the IPoolable components of each object so we don't call GetComponents every time
    readonly Dictionary<GameObject, IPoolable[]> _poolableComponents = new Dictionary<GameObject, IPoolable[]>();
    readonly GameObject _prefab;

    public GameObjectPool(GameObject prefab, int initialCount)
    {
        _prefab = prefab;
        Prewarm(initialCount);
    }

    // Pre-instantiate a set number of objects and deactivate them
    void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Object.Instantiate(_prefab);
            InitializePoolableComponents(go);
            go.SetActive(false);
            _inactiveObjects.Add(go);
        }
    }
    void InitializePoolableComponents(GameObject go)
    {
        _poolableComponents[go] = go.GetComponents<IPoolable>();
        var prefabRef = go.GetComponent<PoolablePrefabRef>();
        if (prefabRef == null) prefabRef = go.AddComponent<PoolablePrefabRef>();
        prefabRef.Prefab = _prefab;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject go;

        // Reuse an existing inactive object if available
        if (_inactiveObjects.Count > 0)
        {
            int lastIndex = _inactiveObjects.Count - 1;
            go = _inactiveObjects[lastIndex];
            _inactiveObjects.RemoveAt(lastIndex);

            // Re-parent and reposition before activating
            if (parent != null)
                go.transform.SetParent(parent, false);
            // Assign position and rotation
            go.transform.SetPositionAndRotation(position, rotation);
        }
        else
        {
            // Instantiate a fresh object if none are inactive
            go = Object.Instantiate(_prefab, position, rotation, parent);
            InitializePoolableComponents(go);
        }

        go.SetActive(true);
        _activeObjects.Add(go);
        // call OnSpawn on IPoolable components of the object
        foreach (IPoolable p in _poolableComponents[go])
        {
            p.OnSpawn();
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        if (_activeObjects.Remove(go))
        {
            go.SetActive(false);
            go.transform.SetParent(null);
            _inactiveObjects.Add(go);
        }
        else
        {
            Debug.LogWarning("Trying to despawn a game object that is not in the active pool.", go);
        }
    }
}

public interface IPoolable
{
    public void OnSpawn();
}
