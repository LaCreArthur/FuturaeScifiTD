using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] float range = 5f;
    [SerializeField] LayerMask enemyMask;

    readonly Collider[] _results = new Collider[10];

    public Transform CurrentTarget { get; private set; }

    void Update()
    {
        if (CurrentTarget != null)
        {
            if (Vector3.Distance(transform.position, CurrentTarget.position) > range)
            {
                CurrentTarget = null;
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

    public void FindTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            range,
            _results,
            enemyMask
        );

        CurrentTarget = count > 0 ? GetPriorityTarget(_results, count) : null;
    }

    Transform GetPriorityTarget(Collider[] candidates, int count) =>
        // Implement different targeting strategies
        candidates[0].transform;
}
