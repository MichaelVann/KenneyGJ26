using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    int m_currentOverlaps;

    internal BoxCollider2D GetCollider() { return GetComponent<BoxCollider2D>(); }
    internal int GetCurrentOverlaps() { return m_currentOverlaps; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_currentOverlaps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D a_collision)
    {
        if (a_collision != null)
        {
            m_currentOverlaps += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D a_collision)
    {
        m_currentOverlaps--;
    }

}
