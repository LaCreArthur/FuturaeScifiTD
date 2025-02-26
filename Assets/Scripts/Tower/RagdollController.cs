using UnityEngine;

public class RagdollController : MonoBehaviour
{

    [SerializeField] Rigidbody impactTargetBone;
    [SerializeField] float impactForceMagnitude = 500f;
    Animator animator;
    Rigidbody[] rigidbodies;
    HealthSystem healthSystem;
    CapsuleCollider _capsuleCollider;

    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.Died -= OnCharacterDied;
        }
    }

    void Start()
    {
        // Cache components
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        // Subscribe to the Died event
        if (healthSystem != null)
        {
            healthSystem.Died += OnCharacterDied;
        }
        else
        {
            Debug.LogError("HealthSystem component not found on " + gameObject.name);
        }

        // Start with ragdoll disabled (kinematic)
        SetRagdollEnabled(false);
    }

    // For testing (optional)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Manual trigger with 'R'
        {
            OnCharacterDied();
        }
    }

    void OnCharacterDied()
    {
        // Trigger ragdoll and apply impact
        EnableRagdoll();

        if (impactTargetBone != null)
        {
            // Example: Apply force upward and slightly backward
            Vector3 impactDirection = (Vector3.up + Vector3.back).normalized;
            impactTargetBone.AddForce(impactDirection * impactForceMagnitude, ForceMode.Impulse);
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
        }
    }
}
