using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    internal static GameHandler s_instance;

    [SerializeField] GameObject _uiFillBarPrefab;
    [SerializeField] TextMeshProUGUI _fpsText;

    vTimer m_fpsTimer;
    List<float> m_fpsTimes;

    float m_levelTimeFactor;


    int m_gameLevel;

    const int m_originalDifficulty = 2;
    int m_levelDifficulty;

    internal GameObject GetUIFillBarPrefab() { return _uiFillBarPrefab; }


    internal int GetGameLevel() { return m_gameLevel; }
    internal int GetLevelDifficulty() { return m_levelDifficulty; }
    internal void SetLevelTimeFactor(float a_levelTimeFactor) { m_levelTimeFactor = a_levelTimeFactor; UpdateTimeScale(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this);
        s_instance = this;
        //QualitySettings.vSyncCount = 0;

        m_gameLevel = 1;

        UpdateLevelDifficulty();

        Application.targetFrameRate = 60;
        m_levelTimeFactor = 1f;

        m_fpsTimer = new vTimer(0.5f);
        m_fpsTimes = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fpsTimes.Add(1f / Time.deltaTime);
        if (m_fpsTimer.Update())
        {
            float total = 0f;
            for (int i = 0; i < m_fpsTimes.Count; ++i)
            {
                total += m_fpsTimes[i];
            }

            _fpsText.text = "FPS: " + VLib.RoundToDecimalPlaces(total/m_fpsTimes.Count, 1);
            m_fpsTimes.Clear();
        }
    }

    void UpdateLevelDifficulty()
    {
        m_levelDifficulty = Mathf.CeilToInt(m_originalDifficulty * m_gameLevel);
    }

    void UpdateTimeScale()
    {
        Time.timeScale = m_levelTimeFactor;
    }

    internal void QuitGame()
    {
        Application.Quit();
    }
}
