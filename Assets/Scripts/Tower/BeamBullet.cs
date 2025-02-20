using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamBullet : MonoBehaviour, IBullet
{
    LineRenderer _lineRenderer;
    Transform _tower;
    Transform _target;
    int _damage;
    float _fireRate;
    EnemyHealthSystem _enemyHealthSystem;

    void Awake() => _lineRenderer = GetComponent<LineRenderer>();

    void Update()
    {
        if (_enemyHealthSystem.CurrentHp <= 0)
        {
            Debug.Log("why", this);
            PoolManager.Despawn(gameObject);
            return;
        }
        if (_fireRate > 0)
        {
            _fireRate -= Time.deltaTime;
            _lineRenderer.SetPosition(0, _tower.position);
            _lineRenderer.SetPosition(1, _target.position);
        }
        else
        {
            Debug.Log("enemy takes damages", this);
            _enemyHealthSystem.TakeDamage(_damage);
            PoolManager.Despawn(gameObject);
        }
    }

    public void Initialize(Transform tower, Transform target, TowerSO towerSO)
    {
        _tower = tower;
        _target = target;
        _damage = towerSO.levels[0].damage;
        _fireRate = towerSO.levels[0].fireRate - 0.2f;

        _lineRenderer.SetPosition(0, _tower.position);
        _lineRenderer.SetPosition(1, _target.position);
        _enemyHealthSystem = _target.GetComponent<EnemyHealthSystem>();
    }
}
