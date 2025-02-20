using UnityEngine;

[RequireComponent(typeof(TargetFinder))]
public class Tower : MonoBehaviour
{
    [SerializeField] TowerSO towerSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform headParent;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float rotationSpeed = 5f;

    TargetFinder _targetFinder;


    float _fireTime;
    int _level;
    Transform CurrentTarget => _targetFinder.CurrentTarget;

    void Awake() => _targetFinder = GetComponent<TargetFinder>();

    void Update()
    {
        if (CurrentTarget == null)
        {
            return;
        }

        LookAtTarget();
        if (_fireTime > 0)
        {
            _fireTime -= Time.deltaTime;
        }
        else
        {
            Shoot();
            _fireTime = fireRate;
        }
    }

    void Shoot()
    {
        // if we have a focused enemy, shoot at it
        if (_targetFinder.CurrentTarget != null)
        {
            foreach (Transform point in spawnPoints)
            {
                SpawnProjectile(bulletPrefab, point.position);
            }
        }
    }

    void SpawnProjectile(GameObject prefab, Vector3 position)
    {
        GameObject bullet = PoolManager.Spawn(prefab, position, Quaternion.identity);
        //todo: actual current level values
        bullet.GetComponent<Bullet>().Initialize(CurrentTarget, towerSO.levels[0].damage, towerSO.levels[0].range);
    }
    void LookAtTarget()
    {
        Vector3 targetDirection = CurrentTarget.position - transform.position;
        targetDirection.y = 0;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            headParent.rotation = Quaternion.Slerp(headParent.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
