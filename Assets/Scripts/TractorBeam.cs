using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [SerializeField] float _forceByMass;
    [SerializeField] Transform _teleportPoint;
    [SerializeField] AudioClip _sellSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D a_collider)
    {
        Rigidbody2D colliderRB = a_collider.gameObject.GetComponent<Rigidbody2D>();
        float force = _forceByMass * colliderRB.mass * Time.fixedDeltaTime;
        Vector3 direction = (transform.position - a_collider.transform.position).normalized;
        colliderRB.AddForce(force * direction);
    }

    private void OnCollisionEnter2D(Collision2D a_collision)
    {
        FallingObject fallingObject = a_collision.gameObject.GetComponent<FallingObject>();
        if (fallingObject)
        {
            fallingObject.GetRigidbody2D().bodyType = RigidbodyType2D.Kinematic;
            fallingObject.transform.position = _teleportPoint.position;
            fallingObject.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;

            BattleHandler.s_instance.ChangeCash((int)fallingObject.GetValue());
            AudioManager.s_instance.PlaySFX(_sellSFX);
        }
    }
}
