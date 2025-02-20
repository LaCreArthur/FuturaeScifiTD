using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour
{
    [SerializeField] int damage;

    void OnTriggerEnter(Collider other) => CheckTrigger(other);

    void CheckTrigger(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem otherHealth))
        {
            otherHealth.TakeDamage(damage);
            PoolManager.Despawn(gameObject);
        }
    }
}
