using UnityEngine;

public class AnagramData
{
    string m_question;
    string m_answer;
    Sprite m_sprite;

    public string question { get { return m_question; } }
    public string answer { get { return m_answer; } }
    public Sprite sprite { get { return m_sprite; } }

    public AnagramData(
        string question,
        string answer,
        Sprite sprite){

        m_question = question;
        m_answer = answer;
        m_sprite = sprite;
    }

}