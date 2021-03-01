using System.Text;
using UnityEngine;


public class ChatData
{
    /// <summary>
    /// 文字色
    /// </summary>
    public Color color;
    
    /// <summary>
    /// ユーザ名
    /// </summary>
    public string username;
    
    /// <summary>
    /// チャットのテキスト
    /// </summary>
    public string text;

    /// <summary>
    /// UI描画時に使用する関数
    /// </summary>
    /// <returns></returns>
    public string GetDrawText()
    {
        string x = text,   c = "",   y = "\n" + "　 ";
        string p = username + "：" + text; 

        foreach (var t in ChatSystem_DataBase.forbiddens)
            x = x.Replace(x, "".PadLeft(x.Length, '*'));
        
        foreach (var t in username)
            y += Encoding.GetEncoding("Shift_JIS").GetByteCount(t + string.Empty) ==  2 ? "　 ": " ";

        for (int b = 0; b < p.Length; b++)
        {
            c += p[b].ToString(); 
            if (b != 0 && b % ChatSystem_DataBase.textPerLine == 0)
                c += y;
        }

        return c;
    }
    

}


