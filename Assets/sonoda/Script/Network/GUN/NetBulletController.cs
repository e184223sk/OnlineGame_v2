using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class NetBulletController :　BulletController
{
    #region Private Properties

    private MonobitView _monobitview;

    //1発当たりのダメージ
    private int _damame = 10;

    #endregion


    #region Unity Callbacks

    /// <summary>
    /// 自身の親もしくは子が持つ MonobitView コンポーネントを探し出す
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

    //現在は銃弾が自分の画面でしか消えない    相手の画面では銃弾は貫通する　どうすれば...
    private void OnCollisionEnter(Collision collision)
    {

        //所有権があるなら
        if (_monobitview.isMine)
        {
            PlayerStatus status = collision.gameObject.GetComponent<PlayerStatus>();
            
            //プレイヤーのステータスクラスがあるなら
            if (status)
            {
                //ダメージを与える
                _monobitview.RPC("Damage", MonobitPlayer.Find(status.GetID()), _damame);
                Init(0, Space.Self);
            }
        }
    }

    #endregion
}
