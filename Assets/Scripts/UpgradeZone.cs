using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UpgradeZone : MonoBehaviour
{
    [SerializeField] Light2D _overheadLight;
    [SerializeField] RectTransform _shutterTransform;
    [SerializeField] float _shutterSpeed, _lightIntensity;
    bool m_playerInZone;

    Player m_player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_playerInZone = false;
        m_player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _overheadLight.intensity = m_playerInZone ? _lightIntensity : 0f;
    }

    private void OnTriggerEnter2D(Collider2D a_collision)
    {
        if (a_collision.gameObject.layer == LayerMask.NameToLayer("Head"))
        {
            m_playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D a_collision)
    {
        if (a_collision.gameObject.layer == LayerMask.NameToLayer("Head"))
        {
            m_playerInZone = false;
        }
    }
}
