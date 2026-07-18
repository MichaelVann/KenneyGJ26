using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : Interactable
{
    Dialogue m_dialogue;

    internal override void Awake()
    {
        base.Awake();
        m_dialogue = new Dialogue("What's up my g? How are you?");
        Dialogue dialogue2 = new Dialogue("Nice. Well, have a good one");
        dialogue2.SetEndsConversation(true);
        Dialogue dialogue3 = new Dialogue("Sorry to hear that. I'm too emotionally unavailable to help sadly.");
        dialogue3.SetEndsConversation(true);

        m_dialogue.AddOption("Good.", dialogue2);
        m_dialogue.AddOption("Terrible to be frank.", dialogue3);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
    }

    internal override void Interact()
    {
        List<string> dialogs = new List<string>();
        dialogs.Add("Test dialog");
        BattleUIHandler.s_instance.StartDialogue(dialogs);
    }
}


