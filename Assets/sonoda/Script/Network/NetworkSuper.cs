using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using System;
public class NetworkSuper :MonobitEngine.MonoBehaviour
{

    //オンラインの最低限な機能をまとめたクラス  　機能：入室、退室、チャット

    //抽象クラスではないが継承して使うことを推奨します

    //チャットはこのままでは使えないので子クラスでメソッドを拡張してください



    #region Static Fields

    MonobitView _view;

    #endregion




    #region Protected Methods

    #region Chat 

    [MunRPC]
    protected virtual void ReceiveChat(string message)
    {

    }


    public virtual void SendChat(string text)
    {
        if (String.IsNullOrEmpty(text))
            return;

        //不適切な表現を削除--------------------------------------------------------------

        text = text.Replace(" ", "").Replace("　", "").Trim();
        if (ForbiddenWords.WordFilter(text))
        {
            text += Environment.NewLine + "<color=red>不適切な表現を含むため送信できません</color>";
            return;
        }

        //---------------------------------------------------------------------------------


        //送信
        _view.RPC("ReceiveChat", MonobitTargets.All, text);
    }


    #endregion



        /// <summary>
        /// roomが存在していればそこに入る。ない場合は新しく作る
        /// </summary>
        /// <param name="room"></param>
    protected virtual void EnterRoom(RoomData room)
    {
        RoomData[] Rooms = MonobitNetwork.GetRoomData();
        if (!MonobitNetwork.inLobby)
        {
            MonobitNetwork.JoinLobby();
            if (Contain(Rooms, room))
            {
                MonobitNetwork.JoinRoom(room.name);
            }
            else
            {
                MonobitNetwork.CreateRoom(room.name);
            }
        }

    }

    protected virtual void LeaveRoom()
    {
        if(MonobitNetwork.inRoom)   MonobitNetwork.LeaveLobby();
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// RoomDataの配列の中に任意のRoomDataが入っているか
    /// </summary>
    /// <param name="Rooms">RoomData配列</param>
    /// <param name="room">調べるRoomData</param>
    /// <returns></returns>
    private bool Contain(RoomData[] Rooms, RoomData room)
    {
        for(int i = 0;i < Rooms.Length; i++)
        {
            if(Rooms[i] ==room)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;


    }


    #endregion



    #region Unity Callbacks
    private void Start()
    {
        if(_view == null)
        {
            _view = this.gameObject.AddComponent<MonobitView>();
        }
    }


    #endregion
}
