using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleUIHandler : MonoBehaviour
{
    internal static BattleUIHandler s_instance;
    [SerializeField] GameObject _uiHudCanvas, _pauseMenu, _pausedText;
    [SerializeField] GameObject _dialoguePrefab;
    [SerializeField] InputActionReference _openMenuAction;
    [SerializeField] TextMeshProUGUI _cashValueText, _debtValueText;
    [SerializeField] Image _debtTimerCircle;



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

        List<string> dialogs = new List<string>();
        dialogs.Add("We've finally caught you! You're stuck here until you pay off your degenerate son's debts! I hear he's still gambling profusely on the underground seahorse races. So it might be a while.");
        dialogs.Add("Catch the fish scales and take them into the cave on the right to sell them. Avoid the big ones for now until you get a few upgrades. And the mines, they'll just send the scales everywhere.");
        dialogs.Add("Also, remember that you've a got nasty allergy to the sand, don't bang your head trying to carry too much. Well, good luck! Make me some money!");
        StartDialogue(dialogs);
    }

    // Update is called once per frame
    void Update()
    {
        int cash = GameHandler.s_instance.GetCash();
        int debt = BattleHandler.s_instance.GetDebt();
        _cashValueText.text = "$" + cash.ToString();
        _cashValueText.color = cash >= debt ?  Color.green : Color.red;
        _debtValueText.text = "$" + debt.ToString();
        _debtTimerCircle.fillAmount = BattleHandler.s_instance.GetDebtTimerFraction();
        _pausedText.SetActive(BattleHandler.s_instance.GetDebtPaused());
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
