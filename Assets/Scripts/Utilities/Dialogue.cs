using System.Collections.Generic;

public class Dialogue
{
    string m_text;
    bool m_endsConversation;

    public struct Option
    {
        internal string text;
        internal Dialogue nextDialogue;
    }

    List<Option> m_options;

    internal void SetEndsConversation(bool a_value) { m_endsConversation = true; }

    public Dialogue(string a_text)
    {
        m_text = a_text;
        m_endsConversation = false;
        m_options = new List<Option>();
    }

    internal void AddOption(string a_text, Dialogue a_nextDialogue)
    {
        m_options.Add(new Option { text = a_text, nextDialogue = a_nextDialogue });
    }

}