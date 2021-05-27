using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Go_Scene_selectButton : MonoBehaviour
{
    public int Select;
    NextSceneEntry next;
    int L_Select;
    Image[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        next = GetComponent<NextSceneEntry>();
        buttons = new Image[14];
        for (int c = 0; c < buttons.Length; c++)
            buttons[c] = GameObject.Find("Canvas/buttons/Image (" + c + ")").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (next.IsFade) return;


        if (Key.A.Down)
        {
            if (Select == buttons.Length - 1)
                next.IsBackTitle = true;
            else
            {
                next.nextRoom = "room" + Select.ToString().PadLeft(2, '0');
                next.IsEnter = true;
            }
        }

        if (Key.JoyStickL.GetRAW.y > 0.4f) Select--;
        else if (Key.JoyStickL.GetRAW.y > 0.4f) Select++;

        if (Select <  0)  Select = buttons.Length - 1;
        else if (Select >= buttons.Length) Select = 0;

        if (Select != L_Select)
        {
            foreach (var t in buttons) t.color = new Color(0.2f, 0.2f, 0.2f, 1);
            buttons[Select].color = Color.white;
            L_Select = Select;
        }
    }
}


//切り替え
//フォーカスを矯正
//エンターで次へ 
//最終デバッグ