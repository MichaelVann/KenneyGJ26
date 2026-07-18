using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Sprite[] _sprites;
    [SerializeField] float _frameInterval;
    [SerializeField] bool _randomiseStartTime = false;
    [SerializeField] bool _destroyingOnComplete = false;

    protected int m_frameIndex;
    vTimer m_frameTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        m_frameIndex = 0;
        m_frameTimer = new vTimer(_frameInterval);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[m_frameIndex];

        if (_randomiseStartTime)
        {
            m_frameIndex = VLib.vRandom(0, _sprites.Length - 1);
            m_frameTimer.SetTimer(VLib.vRandom(0f, m_frameTimer.GetTimerMax()));
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_frameTimer.Update())
        {
            AdvanceFrame();
        }
    }

    protected virtual void AdvanceFrame()
    {
        m_frameIndex++;
        if (_destroyingOnComplete && m_frameIndex >= _sprites.Length)
        {
            Destruct();
        }
        else
        {
            m_frameIndex %= _sprites.Length;
            _spriteRenderer.sprite = _sprites[m_frameIndex];
        }
    }

    void Destruct()
    {
        Destroy(gameObject);
    }
}
