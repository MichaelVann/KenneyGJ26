using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] SpriteRenderer _door;
    [SerializeField] BoxCollider2D _doorCollider;

    Vector3 m_doorOriginalPosition;
    float m_originalDoorHeight, m_doorOpennessRatio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_originalDoorHeight = _door.size.y;
        m_doorOriginalPosition = _door.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _door.size = new Vector2(_door.size.x, m_doorOpennessRatio * m_originalDoorHeight);
        _doorCollider.size =_door.size;
        _door.transform.localPosition = m_doorOriginalPosition + new Vector3(0f, m_originalDoorHeight - _door.size.y)/2f;
        m_doorOpennessRatio = VLib.SinBetween(0f, 1f, 1f, 0f);
    }
}
