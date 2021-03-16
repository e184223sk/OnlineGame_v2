using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif




public class ConectionBehavior : MonobitEngine.MonoBehaviour
{
#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        _monobitView = GetComponent<MonobitView>();
        if (_monobitView == null)
            _monobitView = gameObject.AddComponent<MonobitView>();
        
        Debug.Log("aaa");
       
        //MonobitTransformViewを追加------------------------
        MonobitTransformView t = GetComponent<MonobitTransformView>();
        if (t == null)   t = gameObject.AddComponent<MonobitEngine.MonobitTransformView>();
        _monobitView.ObservedComponents = new List<Component>();
        _monobitView.ObservedComponents.Add(t);

        //MonobitAnimatorViewを追加------------------------
        MonobitAnimatorView a = GetComponent<MonobitAnimatorView>();
        if (GetComponent<Animator>() != null && a == null)
            a = gameObject.AddComponent<MonobitEngine.MonobitAnimatorView>(); 
        if (a != null) _monobitView.ObservedComponents.Add(a);
        //自作クラスの実装

        var sn = GetComponents<SyncNetWorkBehavior>();
        foreach (var snc in sn)
        {
            _monobitView.ObservedComponents.Add(snc); 
        }
       
        if(_monobitView != null)
        _monobitView?.UpdateSerializeViewMethod();
      
    }


#endif

    MonobitView _monobitView;

    [MunRPC]
    static void Receive(object a)
    {
        Debug.Log(a);
    }


    /// <summary>
    /// 親階層子階層のすべてから MonobitViewを探し出す
    /// </summary>
    void Awake()
    {
        // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
        if (GetComponentInParent<MonobitView>() != null)
        {
            _monobitView = GetComponentInParent<MonobitView>();
        }
        // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
        else if (GetComponentInChildren<MonobitView>() != null)
        {
            _monobitView = GetComponentInChildren<MonobitView>();
        }
        // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
        else
        {
            _monobitView = GetComponent<MonobitView>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!MonobitNetwork.isConnect)
        {
            MonobitNetwork.autoJoinLobby = true;
            MonobitNetwork.ConnectServer("testServer");
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
            _monobitView.RPC("Receive", MonobitTargets.All, this.gameObject.GetComponent<MeshRenderer>() as Object);
        }
    }
}

