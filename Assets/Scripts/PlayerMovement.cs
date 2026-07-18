using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject _spriteObj;
    [Header("Movement")]
    [Tooltip("A trigger collider2d below the foot hitbox that acts as an indicator of when the feet are on the ground.")]
    [SerializeField] FootTrigger _footTrigger;
    [SerializeField] InputActionReference _moveAction, _sprintAction;
    [SerializeField] float _movementForceByMass, _sprintForceMult, _airStrafeScale, _maxMoveForce,  _footFriction, _airAccelerationSpeedLimit;
    [SerializeField] float _ladderClimbForce, _ladderStrafeScale, _ladderDrag, _ladderAccelerationSpeedLimit;
    [Header("Jumping")]
    [SerializeField] InputActionReference _jumpAction;
    [SerializeField] float _jumpForceByMass, _jumpResetDelay, _extraJumpMaxVerticalSpeed;
    [SerializeField] int _maxExtraJumps;
    [SerializeField] bool _jumpCancelsDownwardVelocity;



    Rigidbody2D m_rigidBody;

    float m_jumpResetTimer;
    bool m_readyToJump;
    int m_extraJumps;

    int m_laddersTouchedCount;

    internal InputAction GetMovementInputAction() { return _moveAction.action; }

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_laddersTouchedCount = 0;
        m_extraJumps = _maxExtraJumps = 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jumpAction.action.started += Jump;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        LadderUpdate();
        FeetUpdate();
    }

    void LadderUpdate()
    {
        if (m_laddersTouchedCount > 0)
        {
            float climbInput = _moveAction.action.ReadValue<Vector2>().y;
            if (m_rigidBody.linearVelocityY <= 0)
            {
                m_rigidBody.AddForce(-Physics2D.gravity * Time.fixedDeltaTime * 50f);
            }

            m_rigidBody.linearVelocity *= _ladderDrag;

            if (climbInput != 0)
            {
                float speedLimitDelta = _ladderAccelerationSpeedLimit - Mathf.Abs(m_rigidBody.linearVelocityY);
                float ratio = speedLimitDelta / _airAccelerationSpeedLimit;
                ratio = Mathf.Clamp(ratio, 0f, 1f);
                m_rigidBody.AddForceY(_ladderClimbForce * climbInput * Time.fixedDeltaTime * 50f);
            }
        }
    }

    void FeetUpdate()
    {
        bool onGround = m_readyToJump = _footTrigger.GetCurrentOverlaps() > 0;

        if (onGround)
        {
            m_rigidBody.linearVelocity *= new Vector2(_footFriction, 1f);
        }

        if (m_jumpResetTimer < _jumpResetDelay)
        {
            m_jumpResetTimer += Time.deltaTime;
        }
        else if (onGround)
        {
            m_extraJumps = _maxExtraJumps;
        }

        Vector2 moveVector = _moveAction.action.ReadValue<Vector2>();
        moveVector.y = 0f;
        float terrainSpeedBoost = 1f;

        if (!onGround)
        {
            terrainSpeedBoost = m_laddersTouchedCount == 0 ? _airStrafeScale : _ladderStrafeScale;

            if (Mathf.Sign(moveVector.x) == Mathf.Sign(m_rigidBody.linearVelocityX))
            {
                float speedLimitDelta = _airAccelerationSpeedLimit - Mathf.Abs(m_rigidBody.linearVelocityX);
                float ratio = speedLimitDelta/ _airAccelerationSpeedLimit;
                ratio = Mathf.Clamp(ratio, 0f, 1f);
                terrainSpeedBoost *= ratio;
            }
        }

        if (moveVector.x != 0f)
        {
            Vector3 origin = _footTrigger.transform.position;
            origin += new Vector3(0, _footTrigger.GetCollider().size.y / 2f, 0f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, 0.5f, LayerMask.GetMask("Environment"));

            Vector2 normal = hit.collider == null ? Vector2.up : hit.normal;

            float deltaAngle = VLib.FindDeltaAngle(0f, VLib.Vector2ToEulerAngle(normal));
            Vector2 rotatedMoveVector = moveVector.RotateVector2(deltaAngle);
            moveVector = rotatedMoveVector.y > 0 ? moveVector : rotatedMoveVector;

            float maxMoveForce = 15f;

            Vector2 desiredMovement = maxMoveForce * moveVector;

            Vector2 delta = desiredMovement - m_rigidBody.linearVelocity * Mathf.Abs(moveVector.magnitude);
            delta = delta.normalized * Mathf.Clamp(delta.magnitude, -maxMoveForce, maxMoveForce);

            Vector2 moveForce = terrainSpeedBoost * delta / maxMoveForce;
            moveForce *= _sprintAction.action.ReadValue<float>() > 0 ? _sprintForceMult : 1f;

            m_rigidBody.AddForce(moveForce * _movementForceByMass * m_rigidBody.mass * Time.fixedDeltaTime * 50f);

            if (moveVector.x < 0f)
            {
                _spriteObj.transform.localEulerAngles = new Vector3(0f, 0, 0f);
            }
            else if (moveVector.x > 0f)
            {
                _spriteObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }

    void Jump(InputAction.CallbackContext a_context)
    {
        bool jumping = false;
        if (m_readyToJump == true)
        {
            m_readyToJump = false;
            jumping = true;
        }
        else if (m_extraJumps > 0 && m_rigidBody.linearVelocityY <= _extraJumpMaxVerticalSpeed)
        {
            m_extraJumps--;
            jumping = true;
        }

        if (jumping)
        {
            m_jumpResetTimer = 0f;
            float force = _jumpForceByMass * m_rigidBody.mass;
            m_rigidBody.linearVelocityY = _jumpCancelsDownwardVelocity ? 0f : m_rigidBody.linearVelocityY;
            m_rigidBody.AddForce(new Vector2(0f, force));
        }
    }

    private void OnTriggerEnter2D(Collider2D a_collider)
    {
        if (a_collider.gameObject.CompareTag("Ladder"))
        {
            m_laddersTouchedCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D a_collider)
    {
        if (a_collider.gameObject.CompareTag("Ladder"))
        {
            m_laddersTouchedCount--;
        }
    }
}
