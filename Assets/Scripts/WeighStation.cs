using System.Collections.Generic;
using UnityEngine;

public class WeighStation : MonoBehaviour
{
    List<FallingObject> _fallingObjects;

    vTimer m_processTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fallingObjects = new List<FallingObject>();
        m_processTimer = new vTimer(5f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_processTimer.Update())
        {
            Process();
        }
    }

    void Process()
    {
        foreach (FallingObject item in _fallingObjects)
        {
            GameHandler.s_instance.ChangeCash((int)item.GetValue());
            Destroy(item.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _fallingObjects.Add(collision.GetComponent<FallingObject>());
        m_processTimer.Reset();
    }

}
