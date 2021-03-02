using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class NetGunController :GunController
{
    #region Private Properties

    private MonobitView _monobitview;

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

    #endregion

    #region Override Methods

    protected override void NetFire()
    {
        RemainingInMagazines--;
        RemainingOnHand--;
        // FireSe.PlayOneShot(FireSe.clip);// 音
        // ReloadSe.PlayOneShot(ReloadSe.clip);// 音
        GameObject f1 = MonobitNetwork.Instantiate(Bullet_Prefab,BalletSpawnPoint.position,BalletSpawnPoint.rotation,0); //発射
        f1.transform.position = BalletSpawnPoint.position;
        f1.transform.rotation = BalletSpawnPoint.rotation;
        BulletController bc = f1.GetComponent<BulletController>();
        bc.force = new Vector3(bc.force.x, bc.force.y * BulletSpeed, bc.force.z);

        GameObject f2 = MonobitNetwork.Instantiate(Reject_Prefab,RejectCartridgePoint.position,RejectCartridgePoint.rotation,0); //薬莢排出
        f2.transform.position = RejectCartridgePoint.position;
        f2.transform.rotation = RejectCartridgePoint.rotation;
    }

    protected override bool IsMine()
    {
        return _monobitview.isMine;
    }
    #endregion

}
