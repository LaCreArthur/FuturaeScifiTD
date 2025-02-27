using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour
{
    int _damage;
    void OnTriggerEnter(Collider other) => CheckTrigger(other);

    void CheckTrigger(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem otherHealth))
        {
            otherHealth.TakeDamage(_damage);
            PoolManager.Despawn(gameObject);
        }
    }
    public void SetDamage(int damage) => _damage = damage;
}
