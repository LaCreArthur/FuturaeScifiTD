using ScriptableVariables;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem
{
    [SerializeField] FloatVar playerHp;
    [SerializeField] FloatVar playerMaxHp;

    void Awake()
    {
        GameStateManager.OnPlaying += OnLevelStart;
        HpChanged += OnHpChanged;
        MaxHpChanged += OnMaxHpChanged;
    }

    void OnDestroy()
    {
        GameStateManager.OnPlaying -= OnLevelStart;
        HpChanged -= OnHpChanged;
        MaxHpChanged -= OnMaxHpChanged;
    }

    void OnHpChanged(float hp) => playerHp.Value = hp;
    void OnMaxHpChanged(float newMaxHp) => playerMaxHp.Value = newMaxHp;
    void OnLevelStart() => CurrentHp = MaxHp = maxHp;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (CurrentHp <= 0)
        {
            GameStateManager.SetState(GameState.GameOver);
        }
    }
}
