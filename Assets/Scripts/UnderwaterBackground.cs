using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterBackground : MonoBehaviour
{

    [SerializeField] GameObject _fishPrefab;
    [SerializeField] float _maxSpawnY = 15.0f;
    [SerializeField] float _minSpawnTime = 0.1f;
    [SerializeField] float _maxSpawnTime = 1.0f;
    [SerializeField] float _leftXSpawn = -20.0f;
    [SerializeField] float _rightXSpawn = +20.0f;
    vTimer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = new vTimer(_maxSpawnTime);
        SpawnFish();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Update())
        {
            SpawnFish();
            //set a new max time for the itmer
            timer = new vTimer(VLib.vRandom(_minSpawnTime, _maxSpawnTime) / BattleHandler.s_instance.GetDebtLevel());
        }
    }

    void SpawnFish()
    {
        GameObject fishObject = Instantiate(
            _fishPrefab,
            transform.position,
            Quaternion.identity
        );

        BackgroundFish fish = fishObject.GetComponent<BackgroundFish>();

        float randomY = VLib.vRandom(0.0f, _maxSpawnY);
        float xSpawn = fish.IsGoingRight() ? _leftXSpawn : _rightXSpawn;
        fish.transform.position = new Vector3(xSpawn, randomY, transform.position.z); 
    }
}
