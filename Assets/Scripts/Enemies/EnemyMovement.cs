using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This move behavior moves in the direction of the player but will not stop when it reaches the player.
/// </summary>
public class EnemyMovement : MonoBehaviour, IPoolable
{
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float arrivalThreshold = 0.1f;

    float _moveSpeed;
    EnemyHealthSystem _healthSystem;

    List<Vector3> _pathWorldPos = new List<Vector3>();
    int _currentTargetIndex;

    void OnDestroy() => _healthSystem.Died -= OnDeath;

    void Start()
    {
        _healthSystem = GetComponent<EnemyHealthSystem>();
        _healthSystem.Died += OnDeath;
    }

    public void OnSpawn() => StartFollowPath();

    void OnDeath() => StopAllCoroutines();

    void StartFollowPath()
    {
        _currentTargetIndex = 0;
        _pathWorldPos = PathGenerator.PathWorldPositions;
        StartCoroutine(FollowPathRoutine());
    }

    IEnumerator FollowPathRoutine()
    {
        while (_currentTargetIndex < _pathWorldPos.Count)
        {
            Vector3 targetPos = _pathWorldPos[_currentTargetIndex] + Vector3.up * 0.6f;
            while ((transform.position - targetPos).sqrMagnitude > arrivalThreshold)
            {
                MoveTowards(targetPos);
                RotateTowards(targetPos);
                yield return null;
            }
            _currentTargetIndex++;
        }
    }

    void MoveTowards(Vector3 targetPos) => transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
    void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
    public void SetMovementSpeed(float moveSpeed) => _moveSpeed = moveSpeed;
}
