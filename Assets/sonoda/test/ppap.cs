using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppap : SyncNetWorkBehavior
{

    private int _myID;

    private MonobitEngine.MonobitView _monobitView;

    // Start is called before the first frame update
    void Start()
    {

        _monobitView = GetComponent<MonobitEngine.MonobitView>();


        _myID = ID;
        ID++;
    }

    // Update is called once per frame
    void Update()
    {
        if (_monobitView.isMine)
        {
            if (_monobitView.ownerId == _myID)
            {
                Debug.Log("自分のID？" + _monobitView.ownerId);
                if (Input.GetKeyDown(KeyCode.O))
                {
                    MonobitEngine.MonobitNetwork.Instantiate("CUBE", new Vector3(0, 7.6f, 1), Quaternion.identity, 0);
                }
            }


        }

    }
}
