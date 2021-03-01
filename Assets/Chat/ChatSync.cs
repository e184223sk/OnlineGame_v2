using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSync : MonoBehaviour
{
    public static void SEND(ChatUSER user, string text)
    {
        string str0 = user.ToString();
        string str1 = text;
        //各PCのADD関数を引数をそのまま入れて呼ぶ
    }

    public static void RECV(string str0, string str1)
    {
        ChatUSER user = new ChatUSER(str0);
        string text = str1;

        //各PCの
    }
}
