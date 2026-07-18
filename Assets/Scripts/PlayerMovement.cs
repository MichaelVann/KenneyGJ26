using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject _spriteObj;
    [Header("Movement")]
    [SerializeField] InputActionReference _moveAction, _sprintAction;
    [SerializeField] float _movementForceByMass, _sprintForceMult, _airStrafeScale, _maxMoveForce,  _footFriction, _airAccelerationSpeedLimit;


    Rigidbody2D m_rigidBody;


    internal InputAction GetMovementInputAction() { return _moveAction.action; }

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 moveVector = _moveAction.action.ReadValue<Vector2>();
        moveVector.y = 0f;

        if (moveVector.x != 0f)
        {
            float maxMoveForce = 15f;

            Vector2 desiredMovement = maxMoveForce * moveVector;

            Vector2 delta = desiredMovement - m_rigidBody.linearVelocity * Mathf.Abs(moveVector.magnitude);
            delta = delta.normalized * Mathf.Clamp(delta.magnitude, -maxMoveForce, maxMoveForce);

            Vector2 moveForce = delta / maxMoveForce;
            moveForce *= _sprintAction.action.ReadValue<float>() > 0 ? _sprintForceMult : 1f;

            m_rigidBody.AddForce(moveForce * _movementForceByMass * m_rigidBody.mass * Time.fixedDeltaTime * 50f);
        }
        m_rigidBody.linearVelocityX *= _footFriction;
    }

}
