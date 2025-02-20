using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSO towerSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform headParent;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float rotationSpeed = 5f;

    ITargetFinder _targetFinder;
    float _fireRate;
    float _timeUntilNextShot;

    TowerLevel CurrentLevel => towerSO.levels[CurrentLevelIndex]; // Property for current level data
    public int CurrentLevelIndex { get; private set; }
    public int MaxLevel => towerSO.levels.Length - 1;

    void Awake()
    {
        _targetFinder = GetComponent<ITargetFinder>();
        ApplyLevelData(); // Initialize with level 0 data
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

    public void Upgrade()
    {
        if (CurrentLevelIndex < MaxLevel)
        {
            CurrentLevelIndex++;
            ApplyLevelData();
        }
    }

    public int GetUpgradeCost() => CurrentLevelIndex < MaxLevel ? towerSO.levels[CurrentLevelIndex + 1].cost : 0;
    public int GetSellValue() => (int)(CurrentLevel.cost * 0.75f); // Sell for 75% of current level cost

    void ApplyLevelData()
    {
        // Update visuals or other properties if needed (e.g., scale, color)
        _targetFinder.Range = CurrentLevel.range;
        _fireRate = CurrentLevel.fireRate;
    }
}
