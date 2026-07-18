using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _rightingForce, _massPerVolume;
    [SerializeField] Transform[] _headWalls;

    [SerializeField] float _balancePidProportional, _balancePidIntegral, _balancePidDifferential;
    vPID m_balancePID;

    struct Test
    {
        internal float spawnWeight;
    }

    Rigidbody2D m_rigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_balancePID = new vPID(1f, 0f, 0f);
        RefreshCollider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_balancePID.SetVariables(_balancePidProportional, _balancePidIntegral, _balancePidDifferential);
        float currentAngle = VLib.Vector2ToEulerAngle(transform.up);
        float torque = m_balancePID.Update(currentAngle, 0f);
        m_rigidBody.AddTorque(torque * _rightingForce * Time.fixedDeltaTime);
    }

    void RefreshCollider()
    {
        _boxCollider.size = _spriteRenderer.size;
        m_rigidBody.mass = _boxCollider.size.x * _boxCollider.size.y * _massPerVolume;
        _headWalls[0].transform.localPosition = new Vector3(-_boxCollider.size.x/2f, _headWalls[0].transform.localPosition.y);
        _headWalls[1].transform.localPosition = new Vector3(_boxCollider.size.x/2f, _headWalls[0].transform.localPosition.y);
    }
}
