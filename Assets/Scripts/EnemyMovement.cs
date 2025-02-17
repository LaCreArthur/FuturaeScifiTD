using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This move behavior moves in the direction of the player but will not stop when it reaches the player.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float arrivalThreshold = 0.1f;

    List<Vector3> _pathWorldPos = new List<Vector3>();
    int _currentTargetIndex;

    //todo: could be set only once, not when respawned in the same level
    public void SetPath(List<Vector3> pathWorldPos)
    {
        _pathWorldPos = pathWorldPos;
        _currentTargetIndex = 0;
        StartCoroutine(FollowPathRoutine());
    }

    IEnumerator FollowPathRoutine()
    {
        while (_currentTargetIndex < _pathWorldPos.Count)
        {
            Vector3 targetPos = _pathWorldPos[_currentTargetIndex] + Vector3.up;
            while ((transform.position - targetPos).sqrMagnitude > arrivalThreshold)
            {
                MoveTowards(targetPos);
                RotateTowards(targetPos);
                yield return null;
            }
            _currentTargetIndex++;
        }
    }

    void MoveTowards(Vector3 targetPos) => transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
