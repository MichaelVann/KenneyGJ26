using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [SerializeField] float _force;

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
        float force = _force * Time.fixedDeltaTime;
        Vector3 direction = (transform.position - a_collider.transform.position).normalized;
        a_collider.gameObject.GetComponent<Rigidbody2D>().AddForce(force * direction);
    }

    private void OnCollisionEnter2D(Collision2D a_collision)
    {
        FallingObject fallingObject = a_collision.gameObject.GetComponent<FallingObject>();
        if (fallingObject)
        {
            Destroy(fallingObject.gameObject);
            GameHandler.s_instance.ChangeCash(VLib.ChanceTruncate(fallingObject.GetValue()));
        }
    }
}
