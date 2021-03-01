using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class ChatSync : MonobitEngine.MonoBehaviour
{
    #region Public Properties ---------------------------------------------------------

    //MonobitViewコンポーネント
    [SerializeField]
    static MonobitView _monobitview;

    #endregion  ---------------------------------------------------------


    #region Unity CallBacks  ---------------------------------------------------------

    /// <summary>
    /// 親階層子階層のすべてから MonobitViewを探し出す
    /// </summary>
    void Awake()
    {
        // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
        if (GetComponentInParent<MonobitView>() != null)
        {
            _monobitview = GetComponentInParent<MonobitView>();
        }
        // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
        else if (GetComponentInChildren<MonobitView>() != null)
        {
            _monobitview = GetComponentInChildren<MonobitView>();
        }
        // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
        else
        {
            _monobitview = GetComponent<MonobitView>();
        }
    }

    private void Update()
    {
        //--------- ここはチャットのために入室処理をしているが、実際は別のUIで一回だけ処理する必要がある
        //--------- じゃないと、部屋ができるまでの間に結構なエラーを吐く
        if (!MonobitNetwork.isConnect)
        {
            MonobitNetwork.autoJoinLobby = true;
            MonobitNetwork.ConnectServer("ChatServer");
        }
        if (!MonobitNetwork.inRoom)
        {
            //サーバ上の部屋を全取得
            RoomData[] _roomDatas = MonobitNetwork.GetRoomData();
            //部屋が1つでもあるなら
            if (_roomDatas.Length >= 1)
            {
                //1つ目の部屋に入る
                MonobitNetwork.JoinRoom(_roomDatas[0].name);

            }
            else
            {
                Debug.Log("部屋作る");
                //「ChatTest」という名前で部屋を作る
                MonobitNetwork.CreateRoom("ChatTest");

            }
        }


        if (MonobitNetwork.inRoom && Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("送るよ");
            SEND(new ChatUSER("sonoda"), "こんにちは");
        }
    }

    #endregion  ---------------------------------------------------------

    #region Chat Methods ---------------------------------------------------------
    public static void SEND(ChatUSER user, string text)
    {
        string str0 = user.ToString();
        string str1 = text;
        //各PCのADD関数を引数をそのまま入れて呼ぶ
        _monobitview.RPC("RECV", MonobitTargets.All, str0, str1);
        Debug.Log("送信" + str0 + str1);

    }

    [MunRPC]
    public  void RECV(string str0, string str1)
    {
        ChatUSER user = new ChatUSER(str0);
        string text = str1;

        //各PCの

        Debug.Log("username : " + str0 + "   text : " + str1);
    }

    #endregion ---------------------------------------------------------
}
