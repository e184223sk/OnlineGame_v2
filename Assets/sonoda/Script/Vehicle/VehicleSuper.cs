using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class VehicleSuper : MonoBehaviour
{
    //
    #region Public Properties
    //スピードのゲッター変数
    public float _getSpeed { get { return _speed; } }

    //最高速度のゲッター変数
    public int _getMaxSpeed { get { return _MaxSpeed; } }

    //バックの最高速度のゲッター変数
    public int _getMinSpeed { get { return _MinSpeed; } }


    #endregion

    #region Private Variables

    //リジッドボディ
    private Rigidbody _rigidbody;


    //加速に加える力
    [SerializeField]
    private float _AccelPower = 5;

    //加速に加える力
    [SerializeField]
    private float _DecelPower = 5;


    //回転に加える力
    [SerializeField]
    private float _TurnPower = 5;


    //完全静止状態のvelocity.magnitudeの閾値  要るのか？
    [SerializeField,Range(1,5)]
    private float _stopThre = 5;

    //
    private Collider _collider;

    #endregion

    #region Protected Variables

    //乗り物の速度
    [SerializeField]
    protected float _speed;


    //乗り物の最大速度
    [Range(0,150),SerializeField]
    protected int _MaxSpeed;

    //乗り物のバックの最高速度
    [Range(-50,0),SerializeField]
    protected int _MinSpeed;

    [SerializeField]
    protected Vector3 _CenterOfMass;
    

    #endregion


    #region Protected Methods

    protected void Initialized()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _CenterOfMass;
        //mass以外は固定
    }

    protected void Drive()
    {

        //前進後退
        MoveFB();


        //左右に曲がる
        TurnLR();


        _speed = _rigidbody.velocity.magnitude;
        
    }

    #endregion

    #region Private Methods
    //前進
    private void MoveFB()
    {
        Vector2 vertical = Key.JoyStickL.Get;

        //入力がないときに減速処理
        if (Key.JoyStickL.Get.magnitude == 0)
        {
            _rigidbody.AddRelativeForce(new Vector3(0, 0, -vertical.y) * _DecelPower * Time.deltaTime, ForceMode.Acceleration);
        }

        //最高速を上回ったら加速しない
        if (_rigidbody.velocity.magnitude > _MaxSpeed && vertical.y > 0)
            return;

        //最低速を下回ったら減速しない
        if (_rigidbody.velocity.magnitude < _MinSpeed && vertical.y < 0)
            return;
            

        _rigidbody.AddRelativeForce(new Vector3(0, 0, vertical.y) * _AccelPower * Time.deltaTime, ForceMode.Acceleration);


        /*
        //最高速度に対して今どのくらいの速度なのかの割合
        _accelCurveRate = _speed / _MaxSpeed;

        //カーブの位置に応じて加速度を変更
        _accel = _accelCurve.Evaluate( _accelCurveRate )* 25;
        _speed += _accel * Time.deltaTime;

        //最大速度を超えないように
        if (_speed > _MaxSpeed)
            _speed = (float)_MaxSpeed;
        */
    }



    private void TurnLR()
    {
        if (_rigidbody.velocity.magnitude < _stopThre) return;

        Vector2 horizontal = Key.JoyStickL.Get;


        //_rigidbody.AddForce(-Vector3.up * 500* Time.deltaTime,ForceMode.Acceleration);
        _rigidbody.AddRelativeTorque(Vector3.up * horizontal.x * _TurnPower * Time.deltaTime, ForceMode.Acceleration);

    }

    #endregion


    #region Unity Callbacks

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_CenterOfMass, 0.5f);


    }



    #endregion
}
