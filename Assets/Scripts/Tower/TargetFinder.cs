using UnityEngine;

public class TargetFinder : MonoBehaviour, ITargetFinder
{
    //todo: better way for range
    public float range = 5f;
    [SerializeField] LayerMask enemyMask;

    readonly Collider[] _results = new Collider[16];
    public HealthSystem CurrentTargetHealth { get; private set; }

    void Update()
    {
        if (CurrentTarget != null)
        {
            if (Vector3.Distance(transform.position, CurrentTarget.position) > range)
            {
                OnTargetEscapedOrDied();
            }
        }
        else
        {
            FindTarget();
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a semi-transparent sphere when object is selected
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, range);
    }

    public Transform CurrentTarget { get; private set; }

    public void FindTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            range,
            _results,
            enemyMask
        );

        if (count <= 0)
            CurrentTarget = null;
        else
        {
            CurrentTarget = GetClosestTarget(_results);
            CurrentTargetHealth = CurrentTarget.GetComponent<HealthSystem>();
            CurrentTargetHealth.Died += OnTargetEscapedOrDied;
        }
    }

    Transform GetClosestTarget(Collider[] candidates)
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < candidates.Length; i++)
        {
            if (candidates[i] == null) continue;

            float distance = Vector3.Distance(transform.position, candidates[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = candidates[i].transform;
            }
        }

        return closestTarget;

    }

    void OnTargetEscapedOrDied()
    {
        CurrentTargetHealth.Died -= OnTargetEscapedOrDied;
        CurrentTarget = null;
    }
}
