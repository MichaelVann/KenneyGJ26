using UnityEngine;

public class RectSizeOscillator : MonoBehaviour
{
    [SerializeField] float m_scalingMagnitude;
    [SerializeField] float m_speed;
    Vector3 m_originalSize;
    float m_time = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_originalSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.unscaledDeltaTime;
        float sinTime = Mathf.Sin(m_time * m_speed);
        if (sinTime > 0f)
        {
            transform.localScale = m_originalSize * (1f + sinTime * m_scalingMagnitude);
        }
        else
        {
            transform.localScale = m_originalSize * (1f - sinTime * m_scalingMagnitude);
        }
    }
}
