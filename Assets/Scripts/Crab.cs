using UnityEngine;
using static UnityEngine.UI.Image;

public class Crab : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] float _crawlForce, _changeDirectionMinTime, _changeDirectionMaxTime, _rightingTorque, _friction;
    [SerializeField] CapsuleCollider2D _footTrigger;

    float crawlDirection;

    vTimer m_directionChangeCooldownTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        crawlDirection = 1f;
        m_directionChangeCooldownTimer = new vTimer(VLib.vRandom(_changeDirectionMinTime, _changeDirectionMaxTime));
        _rigidBody.centerOfMass = new Vector2(0f, -0.13f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_directionChangeCooldownTimer.Update())
        {
            m_directionChangeCooldownTimer.SetTimerMax(VLib.vRandom(_changeDirectionMinTime, _changeDirectionMaxTime));
            crawlDirection *= -1f;
        }
    }

    private void OnTriggerStay2D(Collider2D a_collision)
    {
        if (LayerMask.LayerToName(a_collision.gameObject.layer) == "Environment")
        {
            _rigidBody.linearVelocity *= 1f - Time.fixedDeltaTime * _friction;

            float force = _crawlForce / (1f + _rigidBody.linearVelocity.magnitude);

            _rigidBody.AddForce(transform.right * force * crawlDirection * Time.fixedDeltaTime);

            Vector2[] normals = new Vector2[3];

            for (int i = 0; i < normals.Length; i++)
            {
                Vector2 origin = transform.position;
                origin.x += -0.41f + i * 0.41f;
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, 0.4f, LayerMask.GetMask("Environment"));
                normals[i] = hit.collider == null ? Vector2.up : hit.normal;
            }

            Vector2 normal = (normals[0] + normals[1] + normals[2]).normalized;

            float deltaAngle = VLib.AngleBetweenVector2s(normal, transform.up);

            _rigidBody.AddTorque(deltaAngle * _rightingTorque * Time.fixedDeltaTime);
        }
    }
}
