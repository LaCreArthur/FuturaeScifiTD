using UnityEngine;

public class ParticleOnDeath : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;

    HealthSystem _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        if (_healthSystem == null) _healthSystem = GetComponentInParent<HealthSystem>();
        if (_healthSystem == null)
        {
            Debug.LogWarning("No HealthSystem found in parent or self", this);
            enabled = false;
            return;
        }
        _healthSystem.Died += OnDeath;
    }


    void OnDeath() => ps.Play();
}
