using System;
using DG.Tweening;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] [ReadOnly] float currentHp;

    Tween _damageTween;

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

        _damageTween?.Kill();
        transform.localScale = Vector3.one;
        _damageTween = transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }
}
