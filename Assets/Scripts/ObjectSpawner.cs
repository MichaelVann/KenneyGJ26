using NUnit.Framework;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering;



public class ObjectSpawner : MonoBehaviour

{
    [SerializeField] GameObject[] _objectPrefabs;
    [SerializeField] float _spawnWidth = 15f;
    [SerializeField] float _maxSpawnImpulse = 5.0f;
    [SerializeField] float _maxSpawnTorque = 1.0f;   
    [SerializeField] float spawnRate = 1.0f;
    vTimer timer;

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        timer = new vTimer(spawnRate);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Update())
        {
            print("spawning object");
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        GameObject fallingObjectPrefab = SelectRandomFallingObject();
        FallingObject fallingObject = Instantiate(fallingObjectPrefab).GetComponent<FallingObject>();

        int spawnX = VLib.vRandom(-15, +15);
        fallingObject.transform.position = new Vector3(spawnX, transform.position.y, 0);

        //add a small rotation impulse 
        float randTorque = VLib.vRandom(-_maxSpawnTorque, +_maxSpawnTorque);
        fallingObject.ApplyTorque(randTorque);

        //add a velocity impulse
        float randImpulse = VLib.vRandom(0, _maxSpawnImpulse);
        Vector3 impulseDir = VLib.vRandom(0,1) > 0.5 ? new Vector3(-1, 0, 0) : new Vector3(+1, 0, 0); //randomly select between impusling left or right
        fallingObject.ApplyForce(randImpulse, impulseDir);
           
    }

    //selects a random falling object based on their spawn weights
    GameObject SelectRandomFallingObject()
    {
        float total = 0f;

        for (int i = 0; i < _objectPrefabs.Length; i++)
        {
            total += _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight();
        }

        float roll = VLib.vRandom(0f, total);
        GameObject selectedPrefab = _objectPrefabs[0];
        for (int i = 0; i < _objectPrefabs.Length; i++)
        {
            if (roll > _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight())
            {
                roll -= _objectPrefabs[i].GetComponent<FallingObject>().GetSpawnWeight();
            }
            else
            {
                selectedPrefab = _objectPrefabs[i];
                break;
            }
        }
        return selectedPrefab;
    }
}
