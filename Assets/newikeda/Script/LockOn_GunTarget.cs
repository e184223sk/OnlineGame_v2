using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn_GunTarget : MonoBehaviour
{
    public Transform targetMarker, HandL, HandR;
    public bool Look,Debeg_LOOK;
    public bool LeftGun, RightGun;
    public MoveJoint IK_Driver;
    public WeaponSelector weaponSelector;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //void FixedUpdate() => Look = false;
    MonobitEngine.MonobitView view;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MonobitEngine.MonobitView>() == null)
            view = GetComponent<MonobitEngine.MonobitView>();

        if (!view.isMine) return;

        if (targetMarker == null)    targetMarker = GameObject.Find("target").transform;
        Vector3 xx = targetMarker.GetComponent<targetPointer>().targetingPOINT;
        weaponSelector.SetTargetPoint(this);

        bool xxv = !(!Look && !Debeg_LOOK);
        if (xxv)
        {
            weaponSelector.weaponA.transform.LookAt(xx);
            weaponSelector.weaponB.transform.LookAt(xx);
        }
        else
        {
            weaponSelector.weaponA.transform.localRotation = Quaternion.Euler(weaponSelector.weaponA.GetComponent<GunBehavior>().layoutAngle);
            weaponSelector.weaponB.transform.localRotation = Quaternion.Euler(weaponSelector.weaponB.GetComponent<GunBehavior>().layoutAngle);
        }

        IK_Driver.handL.enable = xxv && LeftGun;
        IK_Driver.handL.weight = 1;
        if (LeftGun)
            IK_Driver.handL.target.position = (Vector3.Distance(xx, HandL.position) < 0.4f) ? xx : (xx - HandL.position).normalized * 2 + HandL.position;
         
        IK_Driver.handR.enable = xxv && RightGun;
        IK_Driver.handR.weight = 1;
        if (RightGun)
            IK_Driver.handR.target.position = (Vector3.Distance(xx, HandR.position) < 0.4f) ? xx : (xx - HandR.position).normalized * 2 + HandR.position;
    }
}
