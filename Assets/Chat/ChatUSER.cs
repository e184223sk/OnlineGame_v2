using System;
using UnityEngine;

public class ChatUSER 
{
    public string NAME; //名前
    public Color COLOR; //文字の色

    public ChatUSER(string NAME, Color COLOR)
    {
        this.NAME = NAME;
        this.COLOR = COLOR;
    }

    public ChatUSER(string raw)
    {
        string[] r = raw.Split( new string[] { ":.:" }, System.StringSplitOptions.None);
        try
        {
            if (r.Length == 4)
            {
                NAME = r[0];
                COLOR = new Color
                    (
                        float.Parse(r[1]),
                        float.Parse(r[2]),
                        float.Parse(r[3]),
                        float.Parse(r[4])
                    );
            }
        }
        catch (Exception)
        {
            Debug.LogError("CHATDATA");

        }
    }

    public override string ToString()
    {
        return
            NAME + ":.:" +
            COLOR.r.ToString() + ":.:" +
            COLOR.g.ToString() + ":.:" +
            COLOR.b.ToString() + ":.:" +
            COLOR.a.ToString() + ":.:";
    }
}
