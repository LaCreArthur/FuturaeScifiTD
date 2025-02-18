using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float range;
    Vector3 _direction;
    float _distanceTraveled;
    void OnEnable() => _distanceTraveled = 0;
    void Update()
    {
        transform.position += _direction * (moveSpeed * Time.deltaTime);
        _distanceTraveled += moveSpeed * Time.deltaTime;
        if (_distanceTraveled >= range)
        {
            PoolManager.Despawn(gameObject);
        }
    }
    public void Initialize(Vector3 direction)
    {
        _direction = direction;
        transform.LookAt(direction);
    }
}
