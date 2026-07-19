using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject _spriteObj;
    [Header("Movement")]
    [SerializeField] InputActionReference _moveAction, _sprintAction;
    [SerializeField] float _movementForceByMass, _movementForceIncrement, _sprintForceMult, _airStrafeScale, _maxMoveForce,  _airAccelerationSpeedLimit;
    bool m_usingMouseForMovement = true;

    Player m_player;
    Rigidbody2D m_rigidBody;

    internal InputAction GetMovementInputAction() { return _moveAction.action; }

    void Awake()
    {
        m_player = GetComponentInParent<Player>();
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
        if (m_player.GetMovementAllowed())
        {
            Vector2 moveVector = Vector2.zero;

            if (m_usingMouseForMovement)
            {
                if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
                {
                    int pointerId = PointerInputModule.kMouseLeftId;

                    if (!EventSystem.current.IsPointerOverGameObject(pointerId))
                    {
                        moveVector = new Vector2(Mouse.current.leftButton.isPressed ? -1f : 1f, 0f);
                    }
                }
            }
            else
            {
                moveVector = _moveAction.action.ReadValue<Vector2>();
                moveVector.y = 0f;
            }

            if (moveVector.x != 0f)
            {
                float maxMoveForce = 15f;

                Vector2 desiredMovement = maxMoveForce * moveVector;

                Vector2 delta = desiredMovement - m_rigidBody.linearVelocity * Mathf.Abs(moveVector.magnitude);
                delta = delta.normalized * Mathf.Clamp(delta.magnitude, -maxMoveForce, maxMoveForce);

                Vector2 moveForce = delta / maxMoveForce;
                moveForce *= _sprintAction.action.ReadValue<float>() > 0 ? _sprintForceMult : 1f;

                float upgradeBoost = _movementForceByMass + _movementForceIncrement * BattleHandler.s_instance.GetUpgrade(Upgrade.eUpgradeType.MovementForce).GetLevel();

                m_rigidBody.AddForce(moveForce * upgradeBoost * m_rigidBody.mass * Time.fixedDeltaTime * 50f);
            }
        }
    }
}
