using UnityEngine;

public class HeadWall : MonoBehaviour
{
    BoxCollider2D m_boxCollider;
    SpriteRenderer m_spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        RefreshCollider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshCollider()
    {
        m_boxCollider.size = m_spriteRenderer.size;
    }
}
