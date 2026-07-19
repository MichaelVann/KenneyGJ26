using System;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] protected float _value = 1f;
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected float _spawnWeight = 1f;
    [SerializeField] protected float _despawnHeight = -10f;
    [SerializeField] protected float _density = 10.0f;
    [SerializeField] bool _startsFrozen = true;

    internal float GetValue() { return _value; }
    internal Rigidbody2D GetRigidbody2D() { return _rigidbody; }

    internal void SetLinearVelocityX(float a_velocityX) { _rigidbody.linearVelocityX = a_velocityX; }

    bool frozen;

    private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        SetFrozen(_startsFrozen); //Start frozen

        float calculatedWeight = _density * transform.localScale.x * transform.localScale.y;
        _value = _rigidbody.mass = calculatedWeight;
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
        _rigidbody.AddForce(direction.normalized * force, ForceMode2D.Impulse
        );
    }

    public void ApplyTorque(float torque)
    {
        _rigidbody.AddTorque(torque, ForceMode2D.Impulse);
    }

    public float GetSpawnWeight()
    {
        return _spawnWeight;
    }

    public void SetFrozen(bool b)
    {
        frozen = b;

        _rigidbody.bodyType = frozen ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }

    public bool IsFrozen() { return frozen; }

    public float GetMass() { return _rigidbody.mass;  }

}