using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatBox : MonoBehaviour
{
    public Text[] texts;
    public static chatBox box;

    void Start()
    {
        texts = new Text[6];
        box = this;
    }

    public void Add(ChatData chat)
    {
        for (int c = 0; c < 6; c++)
        {
            texts[c].text = texts[c+1].text;
            texts[c].color = texts[c + 1].color;
        }

        texts[6].text = "[" + chat.user + "]" + chat.text;
        texts[6].color = chat.color;
    } 
}

public class ChatData
{
    public string user, text;
    public Color color;
    public ChatData(string user, string text, Color color)
    {
        this.user = user;
        this.text = text;
        this.color = color;
    }
}