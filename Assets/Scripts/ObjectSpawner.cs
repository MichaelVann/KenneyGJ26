using NUnit.Framework;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;



public class ObjectSpawner : MonoBehaviour

{
    [SerializeField] GameObject _objectPrefab;
    [SerializeField] float _spawnWidth = 15.0f;
    [SerializeField] float _maxSpawnImpulse = 5.0f;
    [SerializeField] float _maxSpawnTorque = 1.0f;   

    [SerializeField] float minSpawnRate = 1.0f;
    [SerializeField] float maxSpawnRate = 6.0f;
    vTimer timer;

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        timer = new vTimer(maxSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Update())
        {
            SpawnObject();
            timer = new vTimer(VLib.vRandom(minSpawnRate, maxSpawnRate) / BattleHandler.s_instance.GetDebtLevel());
        }
    }

    void SpawnObject()
    {
        float spawnX = VLib.vRandom(-_spawnWidth, +_spawnWidth);
        Vector3 spawnPos = new Vector3(spawnX, transform.position.y, 0);
        FallingObject fallingObject = Instantiate(_objectPrefab, spawnPos, Quaternion.identity).GetComponent<FallingObject>();

        //add a small rotation impulse 
        float randTorque = VLib.vRandom(-_maxSpawnTorque, +_maxSpawnTorque);
        fallingObject.ApplyTorque(randTorque);

        //add a velocity impulse
        float randImpulse = VLib.vRandom(0, _maxSpawnImpulse);
        Vector3 impulseDir = VLib.vRandom(0,1) > 0.5 ? new Vector3(-1, 0, 0) : new Vector3(+1, 0, 0); //randomly select between impusling left or right
        fallingObject.ApplyForce(randImpulse, impulseDir);
           
    }

    ////selects a random falling object based on their spawn weights
    //GameObject SelectRandomFallingObject()
    //{
    //    float total = 0f;

    //    for (int i = 0; i < _objectPrefabs.Length; i++)
    //    {
    //        total += _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight();
    //    }

    //    float roll = VLib.vRandom(0f, total);
    //    GameObject selectedPrefab = _objectPrefabs[0];
    //    for (int i = 0; i < _objectPrefabs.Length; i++)
    //    {
    //        if (roll > _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight())
    //        {
    //            roll -= _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight();
    //        }
    //        else
    //        {
    //            selectedPrefab = _objectPrefabs[i];
    //            break;
    //        }
    //    }
    //    return selectedPrefab;
    //}
}
