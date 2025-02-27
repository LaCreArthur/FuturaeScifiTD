using DG.Tweening;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem, IPoolable
{
    public GameObject LastBullet { get; set; } // Add this

    public void OnSpawn() => CurrentHp = MaxHp;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (CurrentHp <= 0)
        {
            DOVirtual.DelayedCall(2f, () => PoolManager.Despawn(gameObject));
        }
    }
}
