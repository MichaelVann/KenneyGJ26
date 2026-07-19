using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Head _head;
    [SerializeField] float _explosionForceByMass;
    PlayerMovement m_playerMovement;
    Rigidbody2D m_rigidBody;

    bool m_movementAllowed = true;

    internal bool GetMovementAllowed() { return m_movementAllowed; }

    internal void SetLinearVelocity(Vector2 a_velocity) { m_rigidBody.linearVelocity = a_velocity; }
    internal void SetMovementAllowed(bool a_value) { m_movementAllowed = a_value; }

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void IncreaseHeadSize()
    {
        _head.IncreaseSize();
    }

    internal void Explode()
    {
        Vector2 dir = Vector2.up.RotateVector2(VLib.vRandom(-45f, 45f));

        m_rigidBody.AddForce(dir * _explosionForceByMass * m_rigidBody.mass);
    }

}
