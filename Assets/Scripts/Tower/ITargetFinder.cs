using UnityEngine;

public interface ITargetFinder
{
    Transform CurrentTarget { get; }
    void FindTarget();
}
