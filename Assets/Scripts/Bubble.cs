using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] float _minRiseSpeed = 1.0f;
    [SerializeField] float _maxRiseSpeed = 3.0f;
    [SerializeField] float _despawnY = 80.0f;

    [SerializeField] float wobbleDistance = 5.0f;
    [SerializeField] float wobbleSpeed = 5.0f;

    [SerializeField] float initialSpeedMultiplier = 5.0f;
    [SerializeField] float slowDownSpeed = 4.0f;

    [SerializeField] float _minRandScale = 3.0f;
    [SerializeField] float _maxRandScale = 10.0f;

    float wishRiseSpeed;
    float riseSpeed;
    bool wobbleRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wishRiseSpeed = VLib.vRandom(_minRiseSpeed, _maxRiseSpeed);
        riseSpeed = wishRiseSpeed * initialSpeedMultiplier;

        float randScale = VLib.vRandom(_minRandScale, _maxRandScale);

        transform.localScale = new Vector3(randScale, randScale, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3 (0, riseSpeed, 0);
        riseSpeed = Mathf.Lerp(riseSpeed, wishRiseSpeed, Time.deltaTime * slowDownSpeed);

        if (transform.position.y > _despawnY)
        {
            GameObject.Destroy(gameObject);
        }

        if (wobbleRight)
        {
            transform.position += new Vector3(wobbleSpeed, 0, 0);
            if (transform.position.x > wobbleDistance) wobbleRight = false;
        }

        if (!wobbleRight)
        {
            transform.position -= new Vector3(wobbleSpeed, 0, 0);
            if (transform.position.x < wobbleDistance) wobbleRight = true;
        }

    }
}
