using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class VehicleSuper : MonoBehaviour
{
    //
    #region Getter
    //スピードのゲッター変数
    public float _getSpeed { get { return _speed; } }

    //最高速度のゲッター変数
    public int _getMaxSpeed { get { return _MaxSpeed; } }

    //バックの最高速度のゲッター変数
    public int _getMinSpeed { get { return _MinSpeed; } }


    #endregion

    #region Vehicle Properties

    //リジッドボディ
    private Rigidbody _rigidbody;


    //加速に加える力
    [SerializeField]
    private float _AccelPower = 5;

    //減速に加える力
    [SerializeField]
    private float _DecelPower = 5;


    //回転に加える力
    [SerializeField]
    private float _TurnPower = 5;



    //乗り物の速度
    [SerializeField]
    protected float _speed;

    private float _accel = 0;

    //乗り物の最大速度
    [Range(0, 150), SerializeField]
    protected int _MaxSpeed;

    //乗り物のバックの最高速度
    [Range(-50, 0), SerializeField]
    protected int _MinSpeed;

    [SerializeField]
    protected Vector3 _CenterOfMass;

    //完全静止状態のvelocity.magnitudeの閾値  
    [SerializeField, Range(1, 5)]
    private float _stopThre = 5;


    //自然に減速する量
    [Range(0, 5), SerializeField]
    protected float _inertia;


    //乗り物の加速度
    [SerializeField]
    protected AnimationCurve _accelCurve = null;


    //乗り物の減速度
    [SerializeField]
    protected AnimationCurve _decelCurve = null;

    //アクセルカーブのどこにいるか
    [SerializeField]
    private float _accelCurveRate = 0;

    private float _decelCurveRate = 0;

    [Range(0, 1), SerializeField]
    private float _fadingSpeed = 0.05f;

    [Range(0, 1), SerializeField]
    protected float _fliction;


    //曲がりやすさ
    [Range(0, 100), SerializeField]
    protected float _steering;


    public float PowerTorque = 5;

    #endregion

    #region Wheel Properties

    //タイヤのGameObject f = 前輪　b = 後輪　r = 右側 l = 左側    2輪の場合は_wheel_frと _wheel_br を使う
    private WheelInfo _wheel_fr, _wheel_fl, _wheel_br, _wheel_bl;

    //タイヤ情報の配列
    private WheelInfo[] _wheels;

    private string[] _WheelNames = new string[]
    {
        "wheel_fr",
        "wheel_fl",
        "wheel_br",
        "wheel_bl",
    };

    //4輪かどうか
    [SerializeField]
    protected bool _Is4Wheels = true;

    //前輪の最大回転角
    [SerializeField, Range(0, 90)]
    protected float _MaxSteeringAngle;

    //後輪の最大回転速度
    [SerializeField]
    protected float _MaxMotorTorque;

    #endregion


    #region Initialiezer

    protected void Initialized()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _CenterOfMass;


        //mass以外は固定

        //タイヤ取得   4輪------------------------------------
        if (_Is4Wheels)
        {
            _wheels = new WheelInfo[4];
            for (int i = 0; i < 4; i++)
            {
                _wheels[i] = new WheelInfo(_WheelNames[i], i < 2, i % 2 == 0);
            }
        }
        else  // 2輪
        {
            _wheels = new WheelInfo[2];
            _wheels[0] = new WheelInfo(_WheelNames[0], true, false);
            _wheels[1] = new WheelInfo(_WheelNames[2], false, false);
        }

        foreach (var w in _wheels)
        {
            w._collider.forceAppPointDistance = _CenterOfMass.y - 0.5f;
        }

    }
    #endregion

    #region Driving Methods

    //子クラスで呼ぶ運転メソッド
    protected void Drive()
    {
        //減速処理  速度が0.001を下回ったら停止

        if (!Input.GetKey(KeyCode.W) && _speed > 0)
            _speed = _speed > 0.01 ? _speed - _inertia * Time.deltaTime : 0;
        if (!Input.GetKey(KeyCode.S) && _speed < 0)
            _speed = _speed < -0.01 ? _speed + _inertia * Time.deltaTime : 0;

        //前進後退
        MoveFB();


        //左右に曲がる
        TurnLR();

        //タイヤ制御と左右回転
         WheelCotroll();

        // _rigidbody.velocity *= 0.99f;


        // MoveFB();







        //デバッグ用に出力
        //_speed = _rigidbody.velocity.magnitude;

        // Debug.Log(_rigidbody.velocity);
    }
    //前進
    private void MoveFB()
    {
        float horizontal = Key.JoyStickL.Get.y;

        if (horizontal == 0) return;

        if (horizontal > 0) //前進処理
        {
            //最高速度に対して今どのくらいの速度なのかの割合
            _accelCurveRate = _speed / _MaxSpeed;

            //カーブの位置に応じて加速度を変更
            _accel = _accelCurve.Evaluate(_accelCurveRate) * 25;
            _speed += _accel * Time.deltaTime;

            //最大速度を超えないように
            if (_speed > _MaxSpeed)
                _speed = (float)_MaxSpeed;
        }
        else
        {
            
            //ブレーキ
            if (_speed > 0)
            {
                _decelCurveRate = _speed / _MaxSpeed;
                float breakDis = (_speed * _speed) / (254 * _fliction);

                float a = _speed / (breakDis * 2);
                float decel = _decelCurve.Evaluate(_decelCurveRate) / 10;

                //現在の速度に応じて減速度を増やす
                _speed = _speed > 0.01 ? _speed - decel : 0;

            }
            //後退
            else
            {
                _accel = Mathf.Clamp(_accelCurveRate - _fadingSpeed, 0f, 1f) * 5;
                _speed -= _accel * Time.deltaTime;
            }

            if (_speed < _MinSpeed)
            {
                _speed = _MinSpeed;
            }
        }

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    //右に曲がる
    private void TurnLR()
    {
        if (_speed == 0)
            return;

        transform.Rotate(Vector3.up * _MaxSteeringAngle * Key.JoyStickL.Get.x * Time.deltaTime);
    }

    #endregion


    private void WheelCotroll()
    {

        float horizontal = Key.JoyStickL.Get.x;
        float vertical = Key.JoyStickL.Get.y;
        float steering = _MaxSteeringAngle * horizontal;
        float motor = _MaxMotorTorque * vertical * _AccelPower;

        foreach (var w in _wheels)
        {

            /* Vector3 position;
             Quaternion rotation;
             w._collider.GetWorldPose(out position, out rotation);
             w._object.transform.rotation = rotation;

             */
            Transform transform = w._object.transform;
            transform.Rotate(_speed *_MaxMotorTorque * Time.deltaTime, 0, 0);
            if (w._isFront)
            {
                w._collider.steerAngle = steering;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, w._collider.steerAngle - transform.localEulerAngles.z, transform.localEulerAngles.z);


            }

        }
    }



    #region Unity Callbacks

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_CenterOfMass, 0.5f);

        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(_emphasis, 0.3f);
    }



    #endregion
}
