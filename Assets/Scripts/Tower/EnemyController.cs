using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemySO enemySO;

    EnemyHealthSystem _healthSystem;
    EnemyMovement _movement;
    EnemyAttackBehavior _attackBehavior;
    EnemyGoldSystem _goldSystem; // Optional, if using gold rewards

    void Awake()
    {
        _healthSystem = GetComponent<EnemyHealthSystem>();
        _movement = GetComponent<EnemyMovement>();
        _attackBehavior = GetComponent<EnemyAttackBehavior>();
        _goldSystem = GetComponent<EnemyGoldSystem>();

        if (!_healthSystem || !_movement || !_attackBehavior)
        {
            Debug.LogError("Missing required component on " + gameObject.name);
        }

        _healthSystem.SetMaxHealth(enemySO.maxHealth);
        _movement.SetMovementSpeed(enemySO.moveSpeed);
        _attackBehavior.SetDamage(enemySO.damage);
        _goldSystem.SetGoldReward(enemySO.goldReward);
    }
}
