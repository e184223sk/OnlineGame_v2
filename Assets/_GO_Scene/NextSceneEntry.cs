using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class NextSceneEntry : UnityEngine.MonoBehaviour
{
    public enum CTRLTYPE { TEXTBOX, BUTTONS }
    public CTRLTYPE mode;
    public bool IsFade;
    public bool IsEnter;
    public string nextRoom;
    public bool IsBackTitle;
     
    void INIT()
    {
        GetComponent<Go_Scene_selectButton>().enabled = mode == CTRLTYPE.BUTTONS;
        GetComponent<Go_Scene_textBox>().enabled = mode == CTRLTYPE.TEXTBOX;
        GameObject.Find("Canvas/buttons").active = mode == CTRLTYPE.BUTTONS;
        GameObject.Find("Canvas/INPUT").active = mode == CTRLTYPE.TEXTBOX;
    }
    void Update()
    {
        if (IsFade) return;
        IsFade = IsBackTitle || IsEnter;
        if (IsBackTitle) SceneLoader.Load("SelectScene"); 
        else if(IsEnter) JoinRoom(nextRoom);
    }


    //================================================================

    void Start()
    {
        INIT();


    }

    void JoinRoom(string roomName)
    {
        //roomNameという部屋へ移動
        //あるなら移動ないなら作る

    }
}
