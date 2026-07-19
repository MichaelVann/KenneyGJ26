using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] GameObject _gameHandlerPrefab;
    [SerializeField] Transform _mainCameraPoint, _upgradesCameraPoint;
    [SerializeField] float _cameraLerpSpeed;
    [SerializeField] Player _player;
    [SerializeField] TextMeshProUGUI _cashValueText;
    internal static BattleHandler s_instance;
    float m_pauseMenuTimeFactor, m_dialogueTimeFactor;

    Transform m_targetCameraPoint;


    internal void SetPauseMenuTimeScale(float a_timeFactor) { m_pauseMenuTimeFactor = a_timeFactor; UpdateTimeScale(); }
    internal void SetDialogueTimeScale(float a_timeFactor) { m_dialogueTimeFactor = a_timeFactor; UpdateTimeScale(); }



    private void Awake()
    {
        s_instance = this;
        m_pauseMenuTimeFactor = m_dialogueTimeFactor = 1f;
        if (!GameHandler.s_instance)
        {
            Instantiate(_gameHandlerPrefab);
        }
        m_targetCameraPoint = _mainCameraPoint;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_targetCameraPoint = _player.transform.position.x >= 15f ? _upgradesCameraPoint : _mainCameraPoint;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_targetCameraPoint.position, Time.deltaTime * _cameraLerpSpeed);
        _cashValueText.text = GameHandler.s_instance.GetCash().ToString();
    }

    void UpdateTimeScale()
    {
        Time.timeScale = m_pauseMenuTimeFactor * m_dialogueTimeFactor;
    }

    internal void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }

}
