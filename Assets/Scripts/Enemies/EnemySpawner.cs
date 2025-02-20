using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnRate = 1f;

    float _timeUntilNextSpawn;

    void Awake() => PathGenerator.PathGenerated += OnPathGenerated;
    void OnDestroy() => PathGenerator.PathGenerated -= OnPathGenerated;

    void Update()
    {
        if (GameStateManager.CurrentState != GameState.Playing) return;
        _timeUntilNextSpawn -= Time.deltaTime;
        if (_timeUntilNextSpawn <= 0)
        {
            PoolManager.Spawn(enemyPrefab, transform.position, Quaternion.identity);
            _timeUntilNextSpawn = spawnRate;
        }
    }

    void OnPathGenerated()
    {
        Vector3 firstPosition = PathGenerator.PathWorldPositions[0];
        transform.position = firstPosition + Vector3.up;
    }
}
