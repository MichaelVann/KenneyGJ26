using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour
{
    [SerializeField] Light2D _light;
    [SerializeField] float _flickerChance, _flickerIntensityIncrease, _decayMult;

    float m_defaultIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_defaultIntensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity *= 1f - (Time.deltaTime * _decayMult);
        if (VLib.vRandom(0f, 100f) < _flickerChance)
        {
            _light.intensity += _flickerIntensityIncrease;
        }

        _light.intensity = Mathf.Clamp(_light.intensity, 0f, m_defaultIntensity);
    }
}
