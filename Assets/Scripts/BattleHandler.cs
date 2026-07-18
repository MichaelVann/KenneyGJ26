using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] GameObject _gameHandlerPrefab;
    [SerializeField] Transform _mainCameraPoint, _upgradesCameraPoint, _upgradesPlayerLerpPoint, _battlePlayerLerpPoint;
    [SerializeField] float _cameraLerpSpeed, _playerLerpSpeed;
    [SerializeField] GameObject _transitionToUpgradesTrigger, _transitionToMainTrigger;
    [SerializeField] Player _player;
    internal static BattleHandler s_instance;
    float m_pauseMenuTimeFactor, m_dialogueTimeFactor;

    Transform m_targetCameraPoint;
    bool m_lerpingPlayerIntoUpgrades, m_lerpingPlayerIntoBattle;


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
        _transitionToUpgradesTrigger.SetActive(true);
        _transitionToMainTrigger.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_targetCameraPoint.position, Time.deltaTime * _cameraLerpSpeed);

        if (m_lerpingPlayerIntoUpgrades)
        {
            Vector3 delta = _upgradesPlayerLerpPoint.position - _player.transform.position;
            _player.SetLinearVelocity(delta.normalized * _playerLerpSpeed);
            if (delta.magnitude < 0.1f)
            {
                m_lerpingPlayerIntoUpgrades = false;
                _transitionToMainTrigger.SetActive(true);
                _player.SetMovementAllowed(true);
            }
        }
        else if (m_lerpingPlayerIntoBattle)
        {
            Vector3 delta = _battlePlayerLerpPoint.position - _player.transform.position;
            _player.SetLinearVelocity(delta.normalized * _playerLerpSpeed);
            if (delta.magnitude < 0.1f)
            {
                m_lerpingPlayerIntoBattle = false;
                _transitionToUpgradesTrigger.SetActive(true);
                _player.SetMovementAllowed(true);
            }
        }
    }

    void UpdateTimeScale()
    {
        Time.timeScale = m_pauseMenuTimeFactor * m_dialogueTimeFactor;
    }

    internal void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }

    internal void TransitionToUpgrades()
    {
        m_targetCameraPoint = _upgradesCameraPoint;
        _player.SetMovementAllowed(false);
        m_lerpingPlayerIntoUpgrades = true;
        _transitionToUpgradesTrigger.SetActive(false);
    }

    internal void TransitionToBattle()
    {
        m_targetCameraPoint = _mainCameraPoint;
        m_lerpingPlayerIntoBattle = true;
        _player.SetMovementAllowed(false);
        _transitionToMainTrigger.SetActive(false);
    }

}
