using UnityEngine;

[RequireComponent(typeof(EnemyHealthSystem))]
public class EnemyGoldSystem : MonoBehaviour
{
    [SerializeField] int goldAmount;
    EnemyHealthSystem _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<EnemyHealthSystem>();
        _healthSystem.Died += OnDeath;
    }

    void OnDestroy() => _healthSystem.Died -= OnDeath;
    void OnDeath() => GoldManager.AddGold(goldAmount);
}
