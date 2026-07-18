using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Head _head;
    PlayerMovement m_playerMovement;
    Rigidbody2D m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void IncreaseHeadSize()
    {
        _head.IncreaseSize();
    }
}
