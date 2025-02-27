using UnityEngine;

public class ArtilleryBullet : MonoBehaviour, IBullet
{
    const float MOVE_SPEED = 10f;
    int _damage;
    float _distanceTraveled;
    float _range;
    Transform _target;

    void Update()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        transform.LookAt(direction);
        transform.position += direction * (MOVE_SPEED * Time.deltaTime);
        _distanceTraveled += MOVE_SPEED * Time.deltaTime;

        if (_distanceTraveled >= _range)
        {
            PoolManager.Despawn(gameObject);
        }
        else if ((transform.position - _target.position).sqrMagnitude < 0.1f)
        {
            if (_target.TryGetComponent(out EnemyHealthSystem healthSystem))
            {
                healthSystem.LastBullet = gameObject;
                healthSystem.TakeDamage(_damage);
                PoolManager.Despawn(gameObject);
            }
        }
    }

    public void Initialize(Tower tower, Transform target, TowerSO towerSO)
    {
        _target = target;
        //todo: actual current level values
        _damage = towerSO.levels[tower.CurrentLevelIndex].damage;
        _range = towerSO.levels[tower.CurrentLevelIndex].range;
        _distanceTraveled = 0;
    }
}
