using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class test_init : MonobitEngine.MonoBehaviour
{

    GameObject _playerPre;
    GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _playerPre = Resources.Load("Player") as GameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if (MonobitNetwork.isConnect && MonobitNetwork.inRoom)
        {
            // プレイヤーキャラクタが未登場の場合に登場させる
            if (_player == null)
            {
                _player = MonobitNetwork.Instantiate("Player", transform.position,Quaternion.identity,0);
                GameObject.Find("target").GetComponent<targetPointer>().INIT(_player.transform);
                _player.AddComponent<SendUiParameterFROM_Player>();
              //  
            }
        }
    }
}
