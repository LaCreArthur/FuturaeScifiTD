using UnityEngine;

public interface ITargetFinder
{
    Transform CurrentTarget { get; }
    float Range { get; set; }
    void FindTarget();
}
