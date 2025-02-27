using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemySO[] enemyTypes;
    [SerializeField] int waveSize = 10;
    [SerializeField] float spawnRate = 1f;

    int _waveNumber;
    int _waveEnemyCount;
    float _timeUntilNextSpawn;

    void Awake() => PathGenerator.PathGenerated += OnPathGenerated;
    void OnDestroy() => PathGenerator.PathGenerated -= OnPathGenerated;

    void Update()
    {
        if (GameStateManager.CurrentState != GameState.Playing) return;
        _timeUntilNextSpawn -= Time.deltaTime;
        if (_timeUntilNextSpawn <= 0)
        {
            SpawnEnemy();
            _timeUntilNextSpawn = spawnRate;
        }
    }

    void SpawnEnemy()
    {
        PoolManager.Spawn(enemyTypes[_waveNumber].prefab, transform.position, Quaternion.identity);
        _waveEnemyCount++;
        if (_waveEnemyCount >= waveSize)
        {
            _waveNumber = (_waveNumber + 1) % enemyTypes.Length;
            _waveEnemyCount = 0;
        }
    }

    void OnPathGenerated()
    {
        Vector3 firstPosition = PathGenerator.PathWorldPositions[0];
        transform.position = firstPosition + Vector3.up;
    }
}
