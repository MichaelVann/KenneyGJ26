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

    float moveSpeed = 0.0f;
    public bool goesRight = true;

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

    }

    // Update is called once per frame
    void Update()
    {
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

    public bool IsGoingRight() { return goesRight; }

}
