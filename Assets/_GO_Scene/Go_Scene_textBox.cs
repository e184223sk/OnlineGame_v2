using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Go_Scene_textBox : MonoBehaviour
{
    public InputField i;
    public Text text;
    public NextSceneEntry ns;


    void Start()
    {
        ns = GetComponent<NextSceneEntry>();
        text = GameObject.Find("Canvas/INPUT/Text").GetComponent<Text>();
        i = GameObject.Find("Canvas/INPUT/InputField").GetComponent<InputField>();
        i.ActivateInputField();  
    } 
    

    void Update()
    {
        if (ns.IsBackTitle || ns.IsEnter) return;

        i.ActivateInputField();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (i.text.Length >= 6)
            {
                ns.IsEnter = true;
                ns.nextRoom = i.text;
            }
        }

        if (Key.B.Down)
        {
            ns.IsBackTitle = true;
        }

        text.text = i.text.Length >= 6 ? "" : "ルーム名は6文字以上にしてください";
    }
}
