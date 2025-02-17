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
        enabled = false;
    } 
    
    void OnDestroy()
    {
        PathGenerator.StartCellFound -= OnStartCellFound;
        PathGenerator.PathWorldPosGenerated -= OnPathWorldPosGenerated;
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
    void OnStartCellFound(Vector3 pos) => transform.position = pos + Vector3.up;
    void Spawn()
    {
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        go.GetComponent<EnemyMovement>().SetPath(_pathWorldPos);
    }
    
    void OnPathWorldPosGenerated(List<Vector3> pathWorldPos)
    {
        _pathWorldPos = pathWorldPos;
        enabled = true;
    }
}
