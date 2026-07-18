using UnityEngine;

public class TriggerZone : MonoBehaviour
{

    internal delegate void OnTrigger(Collider2D a_collision);
    OnTrigger m_onTriggerEnterDelegate, m_onTriggerStayDelegate, m_onTriggerExitDelegate;

    internal void SetOnTriggerEnterDelegate(OnTrigger a_delegate) { m_onTriggerEnterDelegate = a_delegate; }
    internal void SetOnTriggerStayDelegate(OnTrigger a_delegate) { m_onTriggerStayDelegate = a_delegate; }
    internal void SetOnTriggerExitDelegate(OnTrigger a_delegate) { m_onTriggerExitDelegate = a_delegate; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D a_collision)
    {
        if (m_onTriggerEnterDelegate != null)
        {
            m_onTriggerEnterDelegate.Invoke(a_collision);
        }
    }

    private void OnTriggerStay2D(Collider2D a_collision)
    {
        if (m_onTriggerStayDelegate != null)
        {
            m_onTriggerStayDelegate.Invoke(a_collision);
        }
    }

    private void OnTriggerExit2D(Collider2D a_collision)
    {
        if (m_onTriggerExitDelegate != null)
        {
            m_onTriggerExitDelegate.Invoke(a_collision);
        }
    }
}
