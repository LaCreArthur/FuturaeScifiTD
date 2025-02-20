using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] [ReadOnly] float currentHp;

    public event Action<float> HpChanged;
    public event Action<float> MaxHpChanged;
    public event Action DamageTaken;
    public event Action Died;

    public float CurrentHp
    {
        get => currentHp;
        protected set {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            HpChanged?.Invoke(currentHp);
        }
    }

    protected float MaxHp
    {
        get => maxHp;
        set {
            maxHp = value;
            MaxHpChanged?.Invoke(maxHp);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        DamageTaken?.Invoke();
        if (CurrentHp <= 0) Died?.Invoke();
    }
}
