using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeighStation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _valueText;
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
        _fallingObjects.Clear();
        RefreshValue();
    }

    void RefreshValue()
    {
        float value = 0f;

        foreach (FallingObject item in _fallingObjects)
        {
            value += (int)item.GetValue();
        }
        _valueText.text = "$" + value.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _fallingObjects.Add(collision.GetComponent<FallingObject>());
        RefreshValue();
        m_processTimer.Reset();
    }

}
