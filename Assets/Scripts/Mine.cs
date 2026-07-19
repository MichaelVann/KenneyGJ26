using System;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Mine : FallingObject
{

    [SerializeField] Light2D _pointLight;
    [SerializeField] Collider2D _explosionCollider;

    [SerializeField] float _lightMinIntensity = 0.3f;
    [SerializeField] float _lightMaxIntensity = 3.0f;
    [SerializeField] float _lightIntensityIncreaseRate = 0.05f;
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] AudioClip _explosionSFX;


    [SerializeField] float _explosionForce = 5.0f;

    bool lightIncreaseIntensity = true;

    [SerializeField] int onExplodeDespawnFrames = 2;
    float onExplosionElapsedFrames = 0f;
    bool collided = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _explosionCollider.gameObject.SetActive(false); 

        float calculatedWeight = transform.localScale.x * transform.localScale.y;
        _rigidbody.mass = calculatedWeight;
    }

    // Update is called once per frame
    void Update()
    {
        //make the light rise and fall 
        _pointLight.intensity = lightIncreaseIntensity ? _pointLight.intensity += _lightIntensityIncreaseRate : _pointLight.intensity -= _lightIntensityIncreaseRate;
        if (_pointLight.intensity >= _lightMaxIntensity) lightIncreaseIntensity = false;
        if (_pointLight.intensity <= _lightMinIntensity) lightIncreaseIntensity = true;

        if (transform.position.y <= _despawnHeight)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (collided)
        {
            onExplosionElapsedFrames += 1;
            if (onExplosionElapsedFrames >= onExplodeDespawnFrames) GameObject.Destroy(gameObject);
        }
    }

    internal void Explode()
    {
        //enable the explosion collision shape
        _explosionCollider.gameObject.SetActive(true);
        AudioManager.s_instance.PlaySFX(_explosionSFX);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody == null) return;

        Player player =  other.gameObject.GetComponentInParent<Player>();
        if (player)
        {
            player.Explode();
            collided = true;
        }
        else
        {
            Vector2 vector = (rigidBody.transform.position - transform.position);
            Vector2 dir = Vector2.up.RotateVector2(VLib.vRandom(-45f, 45f));
            float magnitude = Mathf.Clamp(vector.magnitude, 1f, 100.0f);

            rigidBody.AddForce((dir * _explosionForce) / (magnitude));

            collided = true;
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Falling Object") || collision.gameObject.CompareTag("Player"))
        {
            Explode();
            Instantiate(_explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
