using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float _value = 1f;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _spawnWeight = 1f;
    [SerializeField] float _despawnHeight = -10f;

    private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (transform.position.y <= _despawnHeight)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyForce(float force, Vector2 direction)
    {
        _rigidbody.AddForce(
            direction.normalized * force,
            ForceMode2D.Impulse
        );
    }

    public void ApplyTorque(float torque)
    {
        _rigidbody.AddTorque(
            torque,
            ForceMode2D.Impulse
        );
    }

    public float GetSpawnWeight()
    {
        return _spawnWeight;
    }
}