using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] GameObject _gameHandlerPrefab;
    internal static BattleHandler s_instance;
    float m_pauseMenuTimeFactor, m_dialogueTimeFactor;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
