using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSO towerSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform headParent;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float rotationSpeed = 5f;

    TargetFinder _targetFinder;
    float _fireRate;
    float _timeUntilNextShot;

    void Awake()
    {
        _fireRate = towerSO.levels[0].fireRate;
        _targetFinder = GetComponent<TargetFinder>();
    }

    void Update()
    {
        if (_targetFinder.CurrentTarget == null) return;
        LookAtTarget();

        if (_timeUntilNextShot <= 0)
        {
            Shoot();
            _timeUntilNextShot = _fireRate;
        }
        _timeUntilNextShot -= Time.deltaTime;
    }

    void Shoot()
    {
        foreach (Transform point in spawnPoints)
        {
            GameObject bullet = PoolManager.Spawn(bulletPrefab, point.position, Quaternion.identity);
            if (bullet.TryGetComponent(out IBullet bulletStrategy))
                bulletStrategy.Initialize(transform, _targetFinder.CurrentTarget, towerSO);
        }
    }

    void LookAtTarget()
    {
        Vector3 targetDirection = _targetFinder.CurrentTarget.position - transform.position;
        targetDirection.y = 0;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            headParent.rotation = Quaternion.Slerp(headParent.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
