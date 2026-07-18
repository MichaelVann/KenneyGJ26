using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleUIHandler : MonoBehaviour
{
    internal static BattleUIHandler s_instance;
    [SerializeField] GameObject _uiHudCanvas, _pauseMenu;
    [SerializeField] GameObject _dialoguePrefab;
    [SerializeField] InputActionReference _openMenuAction;

    bool m_dialogueOpen;

    private void Awake()
    {
        s_instance = this;
        _openMenuAction.action.started += TogglePauseMenu;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_dialogueOpen = false;
        SetPauseMenuOpened(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void StartDialogue(List<string> a_strings)
    {
        if (!m_dialogueOpen)
        {
            UIDialogue uiDialogue = Instantiate(_dialoguePrefab, _uiHudCanvas.transform).GetComponent<UIDialogue>();
            uiDialogue.Init(DialogueEnded);
            uiDialogue.AddDialogs(a_strings);
            m_dialogueOpen = true;
            BattleHandler.s_instance.SetDialogueTimeScale(0f);
        }
    }

    void DialogueEnded()
    {
        m_dialogueOpen = false;
        BattleHandler.s_instance.SetDialogueTimeScale(1f);
    }

    void TogglePauseMenu(InputAction.CallbackContext a_context)
    {
        SetPauseMenuOpened(!_pauseMenu.activeSelf);
    }

    public void SetPauseMenuOpened(bool a_open)
    {
        _pauseMenu.SetActive(a_open);
        BattleHandler.s_instance.SetPauseMenuTimeScale(a_open ? 0f : 1f);
    }
}
