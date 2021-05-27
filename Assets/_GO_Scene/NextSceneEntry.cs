using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class NextSceneEntry : UnityEngine.MonoBehaviour
{
    public bool IsFade;
    public bool IsEnter;
    public string nextRoom;
    public bool IsBackTitle;
     
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

    }

    void JoinRoom(string roomName)
    {
        //roomNameという部屋へ移動
        //あるなら移動ないなら作る

    }
}
