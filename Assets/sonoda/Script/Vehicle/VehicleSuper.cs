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

    public float Power = 5;
    public float PowerTorque = 5;

    #endregion

    #region Private Variables
    //アクセルカーブのどこにいるか
    [SerializeField]
    private float _accelCurveRate = 0;

    private float _decelCurveRate = 0;

    [Range(0,1), SerializeField]
    private float _fadingSpeed = 0.05f;

    private float _accel = 0;

    private Rigidbody _rigidbody;
    #endregion

    #region Protected Variables

    //乗り物の速度
    [SerializeField]
    protected float _speed;

    //乗り物の加速度
    [SerializeField]
    protected AnimationCurve _accelCurve = null;


    //乗り物の減速度
    [SerializeField]
    protected AnimationCurve _decelCurve = null; 

    //乗り物の最大速度
    [Range(0,150),SerializeField]
    protected int _MaxSpeed;

    //乗り物のバックの最高速度
    [Range(-50,0),SerializeField]
    protected int _MinSpeed;

    //曲がりやすさ
    [Range(0,100),SerializeField]
    protected float _steering;

    //滑りにくさ
    [Range(0,100),SerializeField]
    protected float _slippery;

    //自然に減速する量
    [Range(0, 5), SerializeField]
    protected float _inertia;

    [Range(0, 1), SerializeField]
    protected float _fliction;

    #endregion


    #region Protected Methods

    protected void Initialized()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //mass以外は固定
    }

    protected void Drive()
    {
        //減速処理  速度が0.001を下回ったら停止
        
        if (!Input.GetKey(KeyCode.W) && _speed > 0) 
            _speed = _speed > 0.01 ? _speed -_inertia * Time.deltaTime : 0;
        if(!Input.GetKey(KeyCode.S) && _speed < 0)
            _speed = _speed < -0.01 ? _speed + _inertia * Time.deltaTime : 0;


        //前進
        if (Input.GetKey(KeyCode.W))
            MoveF();
        //後退
        if (Input.GetKey(KeyCode.S))
            MoveB();

        //左に曲がる
        if (Input.GetKey(KeyCode.A))
            TurnLeft();

        //右に曲がる
        if (Input.GetKey(KeyCode.D))
            TurnRight();
        _rigidbody.AddRelativeForce(Vector3.forward *  _speed * Power * Time.deltaTime, ForceMode.Acceleration); 
    }

    #endregion

    #region Private Methods
    //前進
    private void MoveF()
    {
        //最高速度に対して今どのくらいの速度なのかの割合
        _accelCurveRate = _speed / _MaxSpeed;

        //カーブの位置に応じて加速度を変更
        _accel = _accelCurve.Evaluate( _accelCurveRate )* 25;
        _speed += _accel * Time.deltaTime;

        //最大速度を超えないように
        if (_speed > _MaxSpeed)
            _speed = (float)_MaxSpeed;
    }

    //バック
    private void MoveB()
    {
        if(_speed > 0)
        {
            _decelCurveRate = _speed / _MaxSpeed;
            float breakDis = (_speed * _speed) / (254 * _fliction);

            float a = _speed / (breakDis * 2);


            //現在の速度に応じて減速度を増やす
            _speed = _speed > 0.01 ? _speed - a  /*/ _decelCurve.Evaluate(_decelCurveRate)*/: 0;

        }
            
        else
        {
            _accel = Mathf.Clamp(_accelCurveRate - _fadingSpeed, 0f, 1f) * 5;
            _speed -= _accel * Time.deltaTime;
        }

        if(_speed < _MinSpeed)
        {
            _speed = _MinSpeed;
        }
    }

    //左に曲がる
    private void TurnLeft()
    {
        if (_speed == 0)
            return;
        //_rigidbody.AddRelativeForce(-transform.right * (_speed / _slippery) * Time.deltaTime);
        _rigidbody.AddRelativeTorque(Vector3.up * -_steering * PowerTorque * Time.deltaTime, ForceMode.Acceleration);
    }

    //右に曲がる
    private void TurnRight()
    {
        if (_speed == 0)
            return;
        //_rigidbody.AddRelativeForce(transform.right * (_speed / _slippery)  * Time.deltaTime);
        _rigidbody.AddRelativeTorque(Vector3.up * _steering * PowerTorque * Time.deltaTime, ForceMode.Acceleration); 
    }

    #endregion
}
