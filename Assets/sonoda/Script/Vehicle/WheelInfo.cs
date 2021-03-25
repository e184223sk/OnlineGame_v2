using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//タイヤの情報をまとめるためのクラス
public class WheelInfo : MonoBehaviour
{
    #region Public Properties --------------------------------------------
    public WheelCollider _collider;
    public GameObject _object;
    public bool _isFront;
    public bool _isRight;
    #endregion -----------------------------------------------------------



    #region Public Methods --------------------------------------------

    public WheelInfo(string ObjName, bool IsFront , bool IsRight)
    {
        _collider = GameObject.Find(ObjName + "C").GetComponent<WheelCollider>();
        _object = GameObject.Find(ObjName);
        _isFront = IsFront;
        _isRight = IsRight;
    }

    #endregion -----------------------------------------------------------

}
