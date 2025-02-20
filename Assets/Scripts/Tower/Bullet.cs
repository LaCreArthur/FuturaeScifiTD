using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Transform _target;
    float _distanceTraveled;
    float _range;
    int _damage;

    void OnEnable() => _distanceTraveled = 0;
    void Update()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        transform.LookAt(direction);
        transform.position += direction * (moveSpeed * Time.deltaTime);
        _distanceTraveled += moveSpeed * Time.deltaTime;
        if (_distanceTraveled >= _range)
        {
            PoolManager.Despawn(gameObject);
        }
        if ((transform.position - _target.position).sqrMagnitude < 0.1f)
        {
            if (_target.TryGetComponent(out EnemyHealthSystem healthSystem))
            {
                healthSystem.TakeDamage(_damage);
                PoolManager.Despawn(gameObject);
            }
        }
    }
    public void Initialize(Transform target, int damage, float range)
    {
        _target = target;
        _damage = damage;
        _range = range;
    }
}
