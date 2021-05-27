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
    [Range(-2, 2)]
    public float XX;
    public float cnt;
    [Range(0, 12)]
    public float sensivirity;
    // Start is called before the first frame update
    void Start()
    {
        next = GetComponent<NextSceneEntry>();
        buttons = new Image[14];
        for (int c = 0; c < buttons.Length; c++)
            buttons[c] = GameObject.Find("Canvas/buttons/Image (" + c + ")").GetComponent<Image>();
        L_Select = int.MinValue;
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
        XX = Key.JoyStickL.GetRAW.y;
        if (Key.JoyStickL.GetRAW.y > 0.1f) cnt += Time.deltaTime * XX * sensivirity;
        else if (Key.JoyStickL.GetRAW.y < -0.1f) cnt += Time.deltaTime * XX * sensivirity;
        else cnt = 0;
        if (cnt < -1)
        {
            Select--;
            cnt = 0;
            if (Select < 0)
                Select = buttons.Length - 1;
        }

        if (cnt > 1)
        {
            Select++;
            cnt = 0;
            if (Select >= buttons.Length)
                Select = 0;
        }

         

        if (Select != L_Select)
        {
            foreach (var t in buttons) t.color = new Color(0.2f, 0.2f, 0.2f, 1);
            buttons[Select].color = Color.white;
            L_Select = Select;
        }
    }
}


