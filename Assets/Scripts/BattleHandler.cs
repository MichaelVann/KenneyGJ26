using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] GameObject _gameHandlerPrefab;
    [SerializeField] Transform _mainCameraPoint, _upgradesCameraPoint;
    [SerializeField] float _cameraLerpSpeed;
    [SerializeField] Player _player;
    [SerializeField] float _startingDebtAmount, _startingCashAmount, _debtMult;
    internal static BattleHandler s_instance;
    float m_pauseMenuTimeFactor, m_dialogueTimeFactor;

    Transform m_targetCameraPoint;
    float m_debtAmount;
    vTimer m_debtTimer;

    internal void SetPauseMenuTimeScale(float a_timeFactor) { m_pauseMenuTimeFactor = a_timeFactor; UpdateTimeScale(); }
    internal void SetDialogueTimeScale(float a_timeFactor) { m_dialogueTimeFactor = a_timeFactor; UpdateTimeScale(); }

    internal float GetDebtTimerFraction() { return m_debtTimer.GetCompletionPercentage(); }
    internal int GetDebt() { return (int)m_debtAmount; }


    private void Awake()
    {
        s_instance = this;
        m_pauseMenuTimeFactor = m_dialogueTimeFactor = 1f;
        if (!GameHandler.s_instance)
        {
            Instantiate(_gameHandlerPrefab);
        }
        m_targetCameraPoint = _mainCameraPoint;
        m_debtTimer = new vTimer(60f);
        GameHandler.s_instance.ChangeCash((int)_startingCashAmount);
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

        if (m_debtTimer.Update())
        {
            GameHandler.s_instance.ChangeCash((int)m_debtAmount);
            m_debtAmount = m_debtAmount * _debtMult;
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

}
