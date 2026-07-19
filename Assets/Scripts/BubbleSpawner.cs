using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bubblePrefab;
    [SerializeField] float _minSpawnRate = 4.0f;
    [SerializeField] float _maxSpawnRate = 16.0f;

    vTimer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBubble();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Update())
        {
            SpawnBubble();
        }
    }

    void SpawnBubble()
    {
        Instantiate(_bubblePrefab, transform.position, Quaternion.identity).GetComponent<Bubble>();
        timer = new vTimer(VLib.vRandom(_minSpawnRate, _maxSpawnRate));

    }
}
