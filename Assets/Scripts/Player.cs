using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _glowstickPrefab;
    [SerializeField] float _throwForceByMass;
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] InputActionReference _glowstickAction;
    PlayerMovement m_playerMovement;
    Rigidbody2D m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        _glowstickAction.action.started += ThrowGlowstick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ThrowGlowstick(InputAction.CallbackContext a_context)
    {
        GameObject glowstick = Instantiate(_glowstickPrefab, _projectileSpawnPoint.position, Quaternion.identity);
        float leftRightDirection = _projectileSpawnPoint.transform.position.x > transform.position.x ? 1f : -1f;
        Vector2 movementVector = m_playerMovement.GetMovementInputAction().ReadValue<Vector2>();
        Vector2 throwDirection = Vector2.zero;
        if (movementVector.magnitude == 0)
        {
            throwDirection = Vector2.right;
            throwDirection.x *= leftRightDirection;
        }
        else
        {
            throwDirection = movementVector.normalized;
        }
        Rigidbody2D rb = glowstick.GetComponent<Rigidbody2D>();
        rb.linearVelocity = m_rigidBody.linearVelocity;
        rb.AddForce(throwDirection * _throwForceByMass * rb.mass);
    }
}
