using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UpgradeZone : MonoBehaviour
{
    [SerializeField] Light2D _overheadLight;
    [SerializeField] RectTransform _shutterTransform;
    [SerializeField] float _shutterSpeed;
    bool m_playerInZone;

    float m_originalShutterHeight, m_shutterValue;
    Player m_player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shutterTransform.gameObject.SetActive(true);
        m_playerInZone = false;
        m_shutterValue = 1f;
        m_originalShutterHeight = _shutterTransform.sizeDelta.y;
        m_player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _overheadLight.intensity = m_playerInZone ? 6f : 0f;
        m_shutterValue = Mathf.Lerp(m_shutterValue, m_playerInZone ? 0f : 1f, Time.deltaTime * _shutterSpeed);

        _shutterTransform.sizeDelta = new Vector2(_shutterTransform.sizeDelta.x, m_originalShutterHeight * m_shutterValue);
    }

    private void OnTriggerEnter2D(Collider2D a_collision)
    {
        if (a_collision.gameObject.layer == LayerMask.NameToLayer("Head"))
        {
            m_playerInZone=true;
        }
    }

    private void OnTriggerExit2D(Collider2D a_collision)
    {
        if (a_collision.gameObject.layer == LayerMask.NameToLayer("Head"))
        {
            m_playerInZone = false;
        }
    }

    public void IncreaseHeadSizePressed()
    {
        m_player.IncreaseHeadSize();
    }
}
