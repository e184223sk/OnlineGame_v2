using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class WeaponSelector :MonobitEngine.MonoBehaviour
{
    public int GetSelect { get => Select; }
    public GameObject GetWeapon { get => Select == 0 ? weaponA : weaponB; }

    int Select;
    public GameObject weaponA, weaponB;

    public void SetTargetPoint(LockOn_GunTarget lo)
    {
        var GB_A = weaponA.GetComponent<GunBehavior>();
        if (GB_A != null) GB_A.gunTarget = lo;

        var GB_B = weaponB.GetComponent<GunBehavior>();
        if (GB_B != null) GB_B.gunTarget = lo;
    }

    private void Start()
    { 
        Select = 0;
        weaponA.active = true;
        weaponB.active = false;
        UpdateUserID();
    }

    void Update()
    {
        if (!GetComponent< MonobitEngine.MonobitView >().isMine)
            return;


        UpdateUserID();
        if (Key.Y.Down)
        {
            Select = Select == 0 ? 1 : 0; 
            weaponA.active = Select == 0;
            weaponB.active = !weaponA.active;
        } 
    }

    public void UpdateUserID()
    {

        var id = MonobitEngine.MonobitNetwork.player.ID.ToString();
        GetComponent<PLAYERS>().userID = id;
        if (weaponA.GetComponent<WeaponBehavior>() != null) weaponA.GetComponent<WeaponBehavior>().userID = id;
        if (weaponB.GetComponent<WeaponBehavior>() != null) weaponB.GetComponent<WeaponBehavior>().userID = id; 
            
    }


    public GameObject SetWeapon(GameObject _newWeapon)
    {
        GameObject xx;
        if (Select == 0)
        {
            xx = weaponA;
            _newWeapon.transform.parent = weaponA.transform.parent;
            weaponA = _newWeapon;
            weaponA.GetComponent<GunBehavior>()?.Start();
            weaponA.transform.localPosition = xx.transform.position;
        }
        else
        {
            xx = weaponB;
            _newWeapon.transform.parent = weaponB.transform.parent;
            weaponB = _newWeapon;
            weaponB.GetComponent<GunBehavior>()?.Start(); 
        }
        xx.transform.parent = null;
        UpdateUserID();
        return xx;
    }


    public override void OnMonobitSerializeViewWrite(MonobitStream stream, MonobitMessageInfo info)
    {
        stream.Enqueue(weaponA.transform.localPosition); ;
        stream.Enqueue(weaponA.transform.localRotation); ;

        stream.Enqueue(weaponB.transform.localPosition);
        stream.Enqueue(weaponB.transform.localRotation);
    }

    public override void OnMonobitSerializeViewRead(MonobitStream stream, MonobitMessageInfo info)
    {
        weaponA.transform.localPosition = (Vector3)stream.Dequeue();
        weaponA.transform.localRotation = (Quaternion)stream.Dequeue();

        weaponB.transform.localPosition = (Vector3)stream.Dequeue();
        weaponB.transform.localRotation = (Quaternion)stream.Dequeue();
    }
}


/*
 Z = 射撃
 X = リロード
 C = ジャンプ・上り・梯子
 V = 武器切り替え
 Q = ダッシュ
 Y = しゃがみ
 E = 回収
 R = 
     */
