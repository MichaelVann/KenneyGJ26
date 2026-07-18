using UnityEngine;

public class UiExpandFromNothing : MonoBehaviour
{
    [SerializeField] float _expansionTime;
    [SerializeField] AnimationCurve _expansionCurve;
    Vector3 m_originalScale;

    vTimer m_timer;

    void Awake()
    {
        m_originalScale = transform.localScale;
        m_timer = new vTimer(_expansionTime, true, true, false, true);
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        m_timer.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer.Update();
        transform.localScale = m_originalScale * _expansionCurve.Evaluate(m_timer.GetCompletionPercentage());
    }
}
