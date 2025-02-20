using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float spawnRate;

    float _nextSpawnTime;
    List<Vector3> _pathWorldPos = new List<Vector3>();

    void Awake()
    {
        PathGenerator.StartCellFound += OnStartCellFound;
        PathGenerator.PathWorldPosGenerated += OnPathWorldPosGenerated;
        GameStateManager.OnPlaying += Enable;
        GameStateManager.OnGameOver += Disable;
        enabled = false;
    }

    void Update()
    {
        _nextSpawnTime -= Time.deltaTime;
        if (_nextSpawnTime <= 0)
        {
            Spawn();
            _nextSpawnTime = spawnRate;
        }
    }

    void OnDestroy()
    {
        PathGenerator.StartCellFound -= OnStartCellFound;
        PathGenerator.PathWorldPosGenerated -= OnPathWorldPosGenerated;
        GameStateManager.OnPlaying -= Enable;
        GameStateManager.OnGameOver -= Disable;
    }

    void Enable() => enabled = true;
    void Disable() => enabled = false;
    void OnStartCellFound(Vector3 pos) => transform.position = pos + Vector3.up;
    void Spawn()
    {
        GameObject go = PoolManager.Spawn(prefab, transform.position, Quaternion.identity);
        go.GetComponent<EnemyMovement>().SetPath(_pathWorldPos);
    }

    void OnPathWorldPosGenerated(List<Vector3> pathWorldPos) => _pathWorldPos = pathWorldPos;
}
