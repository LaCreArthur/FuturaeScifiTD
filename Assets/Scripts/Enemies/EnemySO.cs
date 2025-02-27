using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemySO")]
public class EnemySO : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public float moveSpeed;
    public int goldReward;
    public int damageToPlayer;
}
