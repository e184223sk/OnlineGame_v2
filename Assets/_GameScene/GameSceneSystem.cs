using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneSystem : MonoBehaviour
{
    public float time;
    public float start_time;
    TimerUI timerui;
    public ChatUSER user;
    public static GameSceneSystem system;
    public bool IsGameTime;///ゲーム中か?(時間切れや勝利条件を満たしたならfalse)
    void Start()
    {
        system = this;
        time = start_time; 
        IsGameTime = true;
        ChatSystem.userDatas = user;
    }


    void Update()
    {
        if (IsGameTime)
        {
            //時間制限の処理----------------------------------
            time -= Time.deltaTime;
            if (time <= 0)
            {
                IsGameTime = false;
                return;
            }
            //勝利条件/敗北条件を確認する----------------------

            if (false)//どちらかが勝ったらtrueになるif文をかく
            {
                IsGameTime = false;
                return;
            }
        }
        else
        {　
            //終了の処理
        }


    }


#if UNITY_EDITOR
    void OnDrawGizmos() //ギズモではないが...
    {
        time = start_time;
    }

#endif
}
