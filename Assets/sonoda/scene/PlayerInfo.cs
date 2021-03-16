using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo :SyncNetWorkBehavior
{
    MonobitEngine.MonobitView _monobitview;
    public string _string = "";

    private string L_string = "";

    // Start is called before the first frame update
    void Start()
    {
        _monobitview = GetComponent<MonobitEngine.MonobitView>();
    }


    [MunRPC]
     void Receive(string s)
    {
        _string = s;
    }

    // Update is called once per frame
    void Update()
    {
        if(L_string != _string)
        {
            _monobitview.RPC("Receive", MonobitEngine.MonobitTargets.All,_string);
            L_string = _string;
        }
    }
}
