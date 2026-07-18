using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] BoxCollider2D _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider.size = _spriteRenderer.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
