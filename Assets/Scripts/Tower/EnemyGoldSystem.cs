using UnityEngine;

[RequireComponent(typeof(EnemyHealthSystem))]
public class EnemyGoldSystem : MonoBehaviour
{
    EnemyHealthSystem _healthSystem;
    int _goldReward;

    void Awake()
    {
        _healthSystem = GetComponent<EnemyHealthSystem>();
        _healthSystem.Died += OnDeath;
    }

    void OnDestroy() => _healthSystem.Died -= OnDeath;
    void OnDeath() => GoldManager.AddGold(_goldReward);
    public void SetGoldReward(int goldReward) => _goldReward = goldReward;
}
