using System.Threading;
using UnityEngine;

public class BackgroundFish : MonoBehaviour
{
    [SerializeField] Sprite[] _fishSprites;
    [SerializeField] SpriteRenderer _spriteRender;

    [SerializeField] float _minMoveSpeed = 5.0f;
    [SerializeField] float _maxMoveSpeed = 15.0f;

    [SerializeField] float _despawnX = 20.0f;

    [SerializeField] float minScale = 1.0f;
    [SerializeField] float maxScale = 5.0f;

    [SerializeField] GameObject[] _objectPrefabs;

    //time before it drops a scale
    [SerializeField] float _minScaleSpawnTime = 0.5f; 
    [SerializeField] float _maxScaleSpawnTime = 1.5f;

    [SerializeField] float _maxSpawnImpulse = 5.0f;
    [SerializeField] float _maxSpawnTorque = 1.0f;

    float moveSpeed = 0.0f;
    public bool goesRight = true;
    FallingObject heldFallingObject;

    vTimer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //select a random fishSprite
        int randomIndex = UnityEngine.Random.Range(0, _fishSprites.Length-1);   // Pick a random index
        Sprite sprite = _fishSprites[randomIndex];  
        _spriteRender.sprite = sprite;

        //select a random move speed
        moveSpeed = VLib.vRandom(_minMoveSpeed, _maxMoveSpeed);

        //50 50 chance to go left or right
        float randF = VLib.vRandom(0, 1);
        if (randF > 0.5) goesRight = !goesRight;

        //flip the sprite if its going left
        if (!goesRight)
        {
            _spriteRender.transform.localScale = new Vector3(-_spriteRender.transform.localScale.x, 1, 1);
            //flip the position
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }

        //scale the sprite
        _spriteRender.transform.localScale = _spriteRender.transform.localScale * VLib.vRandom(minScale, maxScale);

        //create the timer that will randomly drop scales (fallingObject)
        timer = new vTimer(VLib.vRandom(_minScaleSpawnTime, _maxScaleSpawnTime), true);

        //select a random fallingObject to hold
        GameObject fallingObjectPrefab = SelectRandomFallingObject();
        heldFallingObject = Instantiate(fallingObjectPrefab, transform.position, Quaternion.identity).GetComponent<FallingObject>();


    }

    // Update is called once per frame
    void Update()
    {
        bool inSpawnArea = transform.position.x > -15 && transform.position.x < 15;
        if (timer.Update() && inSpawnArea)
        {
            DropObject();
        }

        if (heldFallingObject.IsFrozen())
        {
            heldFallingObject.transform.position = transform.position;
        }


        if (transform.position.x > _despawnX || transform.position.x < -_despawnX)
        {
            GameObject.Destroy(gameObject);
        }

        if (goesRight)
        {
            transform.position += Vector3.right * (moveSpeed * Time.deltaTime);
        }
        if (!goesRight)
        {
            transform.position -= Vector3.right * (moveSpeed * Time.deltaTime);
        }
    }

    void DropObject()
    {
        heldFallingObject.SetFrozen(false);

        //add a small rotation impulse 
        float randTorque = VLib.vRandom(-_maxSpawnTorque, +_maxSpawnTorque) * heldFallingObject.GetMass();
        heldFallingObject.ApplyTorque(randTorque);

        //add a velocity impulse
        float randImpulse = VLib.vRandom(0, _maxSpawnImpulse) * heldFallingObject.GetMass();
        Vector3 impulseDir = VLib.vRandom(0, 1) > 0.5 ? new Vector3(-1, 0, 0) : new Vector3(+1, 0, 0); //randomly select between impusling left or right
        heldFallingObject.ApplyForce(randImpulse, impulseDir);
    }

    public bool IsGoingRight() { return goesRight; }


    void SpawnObject()
    {
        GameObject fallingObjectPrefab = SelectRandomFallingObject();
        FallingObject fallingObject = Instantiate(fallingObjectPrefab).GetComponent<FallingObject>();

        fallingObject.transform.position = transform.position;

        //add a small rotation impulse 
        float randTorque = VLib.vRandom(-_maxSpawnTorque, +_maxSpawnTorque);
        fallingObject.ApplyTorque(randTorque);

        //add a velocity impulse
        float randImpulse = VLib.vRandom(0, _maxSpawnImpulse);
        Vector3 impulseDir = VLib.vRandom(0, 1) > 0.5 ? new Vector3(-1, 0, 0) : new Vector3(+1, 0, 0); //randomly select between impusling left or right
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



