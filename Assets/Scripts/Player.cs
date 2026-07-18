using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
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
}
