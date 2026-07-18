using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] BoxCollider2D _boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _boxCollider.size = _spriteRenderer.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
