using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;



public class ObjectSpawner : MonoBehaviour

{
    [SerializeField] float _spawnWidth = 15f;
    [SerializeField] GameObject _fallingObjectPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        GameObject obj = Instantiate(_fallingObjectPrefab);
        FallingObject fallingObject = obj.GetComponent<FallingObject>();

        int spawnX = VLib.vRandom(-15, +15);
        fallingObject.transform.position = new Vector3(spawnX, transform.position.y, 0);


        

        return;
        
    }
}
