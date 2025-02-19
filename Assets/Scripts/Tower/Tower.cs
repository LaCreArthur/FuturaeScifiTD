using UnityEngine;

[RequireComponent(typeof(TargetFinder))]
public class Tower : MonoBehaviour
{
    [SerializeField] Transform turretBase;
    [SerializeField] Transform spawnPointL;
    [SerializeField] Transform spawnPointR;
    [SerializeField] GameObject turretRange;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float spawnRate;
    [SerializeField] int layerMask;
    readonly Collider[] _enemiesInRange = new Collider[8];

    TargetFinder _targetFinder;


    float _spawnTime;
    int _level;
    Transform CurrentTarget => _targetFinder.CurrentTarget;

    void Awake() => _targetFinder = GetComponent<TargetFinder>();

    void Update()
    {
        if (CurrentTarget == null)
        {
            return;
        }

        transform.LookAt(CurrentTarget.position);
        if (_spawnTime > 0)
        {
            _spawnTime -= Time.deltaTime;
        }
        else
        {
            Shoot();
            _spawnTime = spawnRate;
        }
    }

    void Shoot()
    {
        // if we have a focused enemy, shoot at it
        if (_targetFinder.CurrentTarget != null)
        {
            Vector3 direction = (CurrentTarget.position - transform.position).normalized;
            GameObject bl = PoolManager.Spawn(bulletPrefab, spawnPointL.position, Quaternion.identity);
            GameObject br = PoolManager.Spawn(bulletPrefab, spawnPointR.position, Quaternion.identity);

            bl.GetComponent<Bullet>().Initialize(direction, _targetFinder.range);
            br.GetComponent<Bullet>().Initialize(direction, _targetFinder.range);
        }
    }

    [ContextMenu("Level Up")]
    void LevelUp()
    {
        if (turretBase == null || turretBase.childCount == 0) return;
        _level = (_level + 1) % turretBase.childCount;
        for (int i = 0; i < turretBase.childCount; i++)
        {
            turretBase.GetChild(i).gameObject.SetActive(i == _level);
        }
    }
}
