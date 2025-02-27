using UnityEngine;

public class RagdollController : MonoBehaviour, IPoolable
{
    [SerializeField] Rigidbody impactTargetBone;
    [SerializeField] float impactForceMagnitude = 500f;
    Animator animator;
    Rigidbody[] rigidbodies;
    HealthSystem healthSystem;
    CapsuleCollider _capsuleCollider;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.Died -= OnCharacterDied;
        }
    }

    void Start()
    {
        // Subscribe to the Died event
        if (healthSystem != null)
        {
            healthSystem.Died += OnCharacterDied;
        }
        else
        {
            Debug.LogError("HealthSystem component not found on " + gameObject.name);
        }
    }

    public void OnSpawn() => DisableRagdoll();

    void OnCharacterDied()
    {
        // Trigger ragdoll and apply impact
        EnableRagdoll();

        if (impactTargetBone != null && healthSystem is EnemyHealthSystem enemyHealth)
        {
            Vector3 forceDirection = Vector3.up; // Default
            if (enemyHealth.LastBullet != null)
            {
                Vector3 bulletDirection = (transform.position - enemyHealth.LastBullet.transform.position).normalized;
                forceDirection = bulletDirection;
            }
            impactTargetBone.AddForce(forceDirection * impactForceMagnitude, ForceMode.Impulse);
        }
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;
        SetRagdollEnabled(true);
    }

    public void DisableRagdoll()
    {
        animator.enabled = true;
        SetRagdollEnabled(false);
    }

    void SetRagdollEnabled(bool enabled)
    {
        _capsuleCollider.enabled = !enabled;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !enabled;
            if (!enabled)
            {
                rb.linearVelocity = Vector3.zero; // Reset velocity when disabling ragdoll
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
