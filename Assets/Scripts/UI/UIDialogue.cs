using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIDialogue : MonoBehaviour
{
    [SerializeField] GameObject m_panelRef;
    [SerializeField] TextMeshProUGUI m_descriptionTextRef;
    [SerializeField] InputActionReference _continueAction;

    string m_descriptionString;
    int m_charactersShown = 0;
    vTimer m_printTimer;
    bool m_printing = false;

    bool m_writingText = false;

    float m_printScreen = 1f;
    const float m_characterPrintTime = 0.03f;

    List<string> m_dialogList;

    float m_flashStrength = 0f;
    const float m_flashDecayMultiplier = 0.99f;

    ZoomExpandComponent m_closingZoom;
    internal delegate void OnCloseDelegate();
    internal OnCloseDelegate m_onCloseDelegate;

    [SerializeField] TextMeshProUGUI m_pressToContinueTextRef;
    bool m_showingPressToContinueText = false;

    internal void AddDialog(string a_string) { m_dialogList.Add(a_string); AssignDescription(); }

    internal void SetPrintSpeed(float a_printSpeed) { m_printScreen = a_printSpeed; RefreshPrintTimer(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ZoomExpandComponent closingZoom = m_panelRef.AddComponent<ZoomExpandComponent>();
        closingZoom.Init();
        closingZoom.SetFinishDelegate(Open);
        _continueAction.action.started += DialogPressed;
    }

    internal void Init(OnCloseDelegate a_onCloseDelegate = null)
    {
        m_onCloseDelegate = a_onCloseDelegate;
        m_dialogList = new List<string>();
        m_descriptionString = "";
        RefreshPrintTimer();
    }

    internal void AddDialogs(List<string> a_dialogs)
    {
        for (int i = 0; i < a_dialogs.Count; i++)
        {
            AddDialog(a_dialogs[i]);
        }
        AssignDescription();
    }

    // Update is called once per frame
    void Update()
    {
        TextUpdate();
        PressToContinueTextUpdate();
    }

    void RefreshPrintTimer()
    {
        m_printTimer = new vTimer(m_characterPrintTime / m_printScreen, false, true, true, true);
    }

    void AssignDescription()
    {
        m_descriptionString = m_dialogList[0];
        m_descriptionTextRef.text = m_descriptionString;

        m_charactersShown = 0;
        m_descriptionTextRef.maxVisibleCharacters = m_charactersShown;

        RefreshPrintTimer();
    }

    void PressToContinueTextUpdate()
    {
        if (m_showingPressToContinueText)
        {
            float sinT = Mathf.Abs(Mathf.Sin(Time.time * 2f)) * 0.5f + 0.5f;
            m_pressToContinueTextRef.color = new Color(sinT, sinT, sinT, sinT);
        }
    }

    void TextUpdate()
    {
        if (m_writingText && m_descriptionString != null && m_charactersShown < m_descriptionString.Length)
        {
            if (m_printTimer.Update())
            {
                m_charactersShown++;
                m_descriptionTextRef.maxVisibleCharacters = m_charactersShown;

                if (m_descriptionTextRef.text[m_descriptionTextRef.text.Length - 1] != ' ')
                {
                    m_flashStrength = 1f;
                }
            }
            m_printing = true;
        }
        else
        {
            m_printing = false;
        }

        m_showingPressToContinueText = !m_printing;
        m_flashStrength *= m_flashDecayMultiplier;
    }

    void ProgressDialog()
    {
        if (m_dialogList.Count > 1)
        {
            m_dialogList.RemoveAt(0);
            AssignDescription();
        }
        else if (m_closingZoom == null)
        {
            m_closingZoom = m_panelRef.AddComponent<ZoomExpandComponent>();
            m_closingZoom.Init(1f, 0f, 0.3f, 2f, Close);
        }
    }
    void DialogPressed(InputAction.CallbackContext a_context)
    {
        DialogPressed();
    }

    public void DialogPressed()
    {
        if (m_printing)
        {
            m_charactersShown = m_descriptionString.Length - 1;
        }
        else
        {
            ProgressDialog();
        }
    }

    void Open()
    {
        m_writingText = true;
    }

    void Close()
    {
        if (m_onCloseDelegate != null)
        {
            m_onCloseDelegate.Invoke();
        }
        _continueAction.action.started -= DialogPressed;

        Destroy(gameObject);
    }
}
