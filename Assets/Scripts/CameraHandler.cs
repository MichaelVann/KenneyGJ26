using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _lerpSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraOffset = new Vector3(0f, 0f, -10f);
        transform.position = Vector3.Lerp(transform.position, _playerTransform.position + cameraOffset, Time.deltaTime * _lerpSpeed);
    }
}
