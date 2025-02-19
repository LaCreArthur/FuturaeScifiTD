using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Vector3 _direction;
    float _distanceTraveled;
    float _range;

    void OnEnable() => _distanceTraveled = 0;
    void Update()
    {
        transform.position += _direction * (moveSpeed * Time.deltaTime);
        _distanceTraveled += moveSpeed * Time.deltaTime;
        if (_distanceTraveled >= _range)
        {
            PoolManager.Despawn(gameObject);
        }
    }
    public void Initialize(Vector3 direction, float range)
    {
        _direction = direction;
        _range = range;
        transform.LookAt(direction);
    }
}
