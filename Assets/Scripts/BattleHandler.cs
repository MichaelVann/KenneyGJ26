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
    bool m_debtPaused;
    vTimer m_debtTimer;
    
    Upgrade[] m_upgrades;

    internal void SetPauseMenuTimeScale(float a_timeFactor) { m_pauseMenuTimeFactor = a_timeFactor; UpdateTimeScale(); }
    internal void SetDialogueTimeScale(float a_timeFactor) { m_dialogueTimeFactor = a_timeFactor; UpdateTimeScale(); }

    internal float GetDebtTimerFraction() { return m_debtTimer.GetCompletionPercentage(); }
    internal int GetDebt() { return (int)m_debtAmount; }

    internal Upgrade GetUpgrade(int a_index) { return m_upgrades[a_index]; }
    internal Upgrade GetUpgrade(Upgrade.eUpgradeType a_type) { return m_upgrades[(int)a_type]; }

    internal bool GetDebtPaused() { return m_debtPaused; }

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
        m_debtAmount = _startingDebtAmount;

        m_upgrades = new Upgrade[(int)Upgrade.eUpgradeType.Count];
        for (int i = 0; i < m_upgrades.Length; i++)
        {
            m_upgrades[i] = new Upgrade();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool inUpgradeZone = _player.transform.position.x >= 15f;
        m_debtPaused = inUpgradeZone;
        m_targetCameraPoint = inUpgradeZone ? _upgradesCameraPoint : _mainCameraPoint;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_targetCameraPoint.position, Time.deltaTime * _cameraLerpSpeed);

        if (!m_debtPaused && m_debtTimer.Update())
        {
            GameHandler.s_instance.ChangeCash((int)-m_debtAmount);
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
