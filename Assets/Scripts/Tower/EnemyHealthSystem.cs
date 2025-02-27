using DG.Tweening;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem, IPoolable
{
    [SerializeField] EnemySO enemySO;
    public GameObject LastBullet { get; set; } // Add this
    void Awake() => MaxHp = enemySO.maxHealth;
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
