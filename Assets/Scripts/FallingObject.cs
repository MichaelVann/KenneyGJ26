using System;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    float value = 1f;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _spawnWeight = 1.0f;
    [SerializeField] float _despawnHeight = -10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody.transform.position.y <= _despawnHeight)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void ApplyForce(float force, Vector2 direction)
    {
        _rigidbody.AddForce(direction * force);
    }

    public void ApplyTorque(float torque)
    {
        _rigidbody.AddTorque(torque);
    }

    public float GetSpawnWeight()
    {
        return _spawnWeight;
    }
   
}
